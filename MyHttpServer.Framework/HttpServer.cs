using System.Net;
using System.Text;
using System.Text.Json;
using MyHttpServer.Framework.Authentication;
using MyHttpServer.Framework.Handlers;

namespace MyHttpServer;

public class HttpServer
{
    private HttpListener server;
    private bool _isListening;
    private readonly string _dbConnection;
    private readonly IUserSessionService _sessionService;
    
    public HttpServer(Settings settings)
    {
        server = new HttpListener();
        server.Prefixes.Add($"http://{settings.Server.Host}:{settings.Server.Port}/{settings.Server.Path}");
        _dbConnection = settings.Database.ConnectionString;
        _sessionService = new UserSessionService();
    }
    
    
    public void Start()
    {
        server.Start();
        _isListening = true;
        Console.WriteLine("Сервер запущен");
        Listen();
    }
    
    private async Task Listen()
    {
        while (_isListening)
        {
            try
            {
                var context = await server.GetContextAsync();
                _ = ProcessRequestAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            Handler staticFileHandler = new StaticFileHandler();
            Handler controllerHandler = new ControllerHandler(_dbConnection, _sessionService);
            Handler notFoundHandler = new NotFoundHandler();
            staticFileHandler.Successor = controllerHandler;
            controllerHandler.Successor = notFoundHandler;

            Console.WriteLine($"Пришёл запрос {context.Request.Url}");
            await staticFileHandler.HandleRequest(context);

            Console.WriteLine($"Обработан запрос {context.Request.Url}");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            Console.WriteLine(ex.ToString());
            context.Response.Close();
        }
    }
 
    public void Stop()
    {
        _isListening = false;
        server.Stop();
        Console.WriteLine("Сервер выключен");
    }
}