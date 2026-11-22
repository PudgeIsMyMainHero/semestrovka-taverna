
using System.Net;
using System.Reflection;
using System.Text;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.Authentication;
using MyHttpServer.Framework.HttpResponses;

namespace MyHttpServer.Framework.Handlers;

public class ControllerHandler : Handler
{
    private readonly string _connectionString;
    private readonly IUserSessionService _sessionService;

    public ControllerHandler(string connectionString, IUserSessionService sessionService)
    {
        _connectionString = connectionString;
        _sessionService = sessionService;
    }
    public override async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        string[] strParams = request.Url!
            .Segments
            .Skip(1)
            .Select(s => s.Trim('/'))
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();

        string controllerRoute = strParams.Length > 0 ? strParams[0] : "";

        string actionRoute = string.Join("/", strParams.Skip(1));

        var assembly = Assembly.GetEntryAssembly();

        var controller = assembly.GetTypes()
            .FirstOrDefault(t =>
            {
                var attr = t.GetCustomAttribute<HttpControllerAttribute>();

                return attr != null &&
                    attr.Route.Equals(
                        controllerRoute,
                        StringComparison.OrdinalIgnoreCase);
            });

        if (controller == null)
        {
            if (Successor != null)
                await Successor.HandleRequest(context);
            else
            {
                response.StatusCode = 404;
                response.Close();
            }

            return;
        }

        var method = controller.GetMethods()
            .FirstOrDefault(m =>
            {
                string? route = null;

                if (request.HttpMethod == "GET")
                {
                    route = m.GetCustomAttribute<GetAttribute>()?.Route;
                }
                else if (request.HttpMethod == "POST")
                {
                    route = m.GetCustomAttribute<PostAttribute>()?.Route;
                }
                return route != null && route.Equals(
                        actionRoute,
                        StringComparison.OrdinalIgnoreCase);
            });

        if (method == null)
        {
            if (Successor != null)
                await Successor.HandleRequest(context);
            else
            {
                response.StatusCode = 404;
                response.Close();
            }

            return;
        }

        var access = CheckAccess(context, controller, method);

        if (access != AccessResult.Allowed)
        {
            response.StatusCode = access == AccessResult.Unauthenticated ? 401 : 403;
            string deniedPagePath = access == AccessResult.Unauthenticated
                ? "static/401.html"
                : "static/403.html";

            string html = await File.ReadAllTextAsync(deniedPagePath);
            byte[] deniedBuffer = Encoding.UTF8.GetBytes(html);
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = deniedBuffer.Length;

            await response.OutputStream.WriteAsync(
                deniedBuffer);
            context.Response.Close();

            return;
        }

        var parameters = method.GetParameters();
        var query = request.QueryString;

        var form = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

        bool needsForm = parameters.Any(p =>
            p.GetCustomAttribute<FromFormAttribute>() != null);

        if (needsForm)
        {
            if (request.ContentType == null ||
                !request.ContentType.StartsWith(
                    "application/x-www-form-urlencoded",
                    StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = 415;
                response.Close();
                return;
            }

            using var reader = new StreamReader(
                request.InputStream,
                request.ContentEncoding);

            string body = await reader.ReadToEndAsync();

            foreach (string pair in body.Split('&'))
            {
                if (string.IsNullOrEmpty(pair))
                    continue;

                string[] parts = pair.Split('=', 2);

                string key = WebUtility.UrlDecode(parts[0]);
                string value = parts.Length > 1
                    ? WebUtility.UrlDecode(parts[1])
                    : "";

                form[key] = value;
            }
        }

        object[] args;

        try
        {
            args = parameters.Select(p =>
            {
                if (p.ParameterType == typeof(HttpListenerContext))
                    return context;

                string? value;

                if (p.GetCustomAttribute<FromQueryAttribute>() != null)
                {
                    value = query[p.Name!];
                }
                else if (p.GetCustomAttribute<FromFormAttribute>() != null)
                {
                    form.TryGetValue(p.Name!, out value);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"У параметра {p.Name} не указан источник данных");
                }

                if (value == null)
                {
                    if (p.HasDefaultValue)
                        return p.DefaultValue!;
                    throw new InvalidOperationException(
                        $"Не найден параметр: {p.Name}");
                }

                Type targetType =
                    Nullable.GetUnderlyingType(p.ParameterType)
                    ?? p.ParameterType;

                return Convert.ChangeType(value, targetType)!;
            }).ToArray();
        }
        catch (Exception ex)
        {
            response.StatusCode = 400;
            Console.WriteLine(ex.ToString());
            response.Close();
            return;
        }

        var controllerInstance = Activator.CreateInstance(controller);

        if (controllerInstance is ControllerBase controllerBase)
        {
            controllerBase.Initialize(context, _sessionService, _connectionString);
        }
        var result = method.Invoke(controllerInstance, args);

        if (result is Task task)
        {
            await task;

            var resultProperty = task.GetType()
                .GetProperty("Result");

            result = resultProperty?.GetValue(task);
        }

        if (result is IHttpResponseTypeResult responseResult)
        {
            try
            {
                await responseResult.ExecuteAsync(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.StatusCode = 500;
            }

            response.Close();
            return;
        }
        
        if (result is string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            if (response.ContentType == null)
            {
                response.ContentType = request.HttpMethod == "GET"
                    ? "text/html; charset=utf-8"
                    : "text/plain; charset=utf-8";
            }
            response.ContentLength64 = buffer.Length;

            await response.OutputStream.WriteAsync(buffer);
            response.Close();
            return;
        }
        await Successor.HandleRequest(context);
        response.Close();
    }

    private enum AccessResult
    {
        Allowed,
        Unauthenticated,
        Forbidden,
    }

    private AccessResult CheckAccess(
        HttpListenerContext context,
        Type controllerType,
        MethodInfo method)
    {
        bool requiresAdmin =
            controllerType.IsDefined(
                typeof(RequireAdminAttribute), true)
            || method.IsDefined(
                typeof(RequireAdminAttribute), true);

        bool requiresAuthorization =
            requiresAdmin
            || controllerType.IsDefined(
                typeof(AuthorizeAttribute), true)
            || method.IsDefined(
                typeof(AuthorizeAttribute), true);
        

        if (!requiresAuthorization)
        {
            return AccessResult.Allowed;
        }

        string? sessionId =
            context.Request.Cookies["sessionId"]?.Value;

        int? userId =
            _sessionService.GetUserId(sessionId);

        if (!userId.HasValue)
        {
            return AccessResult.Unauthenticated;
        }

        if (requiresAdmin && _sessionService.GetIsAdmin(sessionId) != true)
        {
            return AccessResult.Forbidden;
        }

        return AccessResult.Allowed;
    }
}
