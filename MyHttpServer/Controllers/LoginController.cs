using System.Net;
using MyHttpServer.Entities;
using MyHttpServer.Framework;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.HttpResponses;
using MyHttpServer.Services;
using ORM;

namespace MyHttpServer.Controllers;

[HttpController("login")]
public class LoginController : ControllerBase
{
    [Get("")]
    public async Task<IHttpResponseTypeResult> Login()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "static", "login.html");
        if (File.Exists(path))
        {
            string html = await File.ReadAllTextAsync(path);
            return Html(html);
        }

        return null!;
    }

    [Post("auth")]
    public async Task<IHttpResponseTypeResult> Login(
        [FromForm] string login,
        [FromForm] string password)
    {
        var orm = new OrmContext(ConnectionString);
        var users = await orm.GetAllAsync<User>("users");

        var user = users.FirstOrDefault(u =>
            u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
            PasswordHasher.Verify(password, u.Password));

        if (user == null)
        {
            return Redirect("/login");
        }

        string sessionId = SessionService.CreateSession(user.Id, user.IsAdmin);

        var cookie = new Cookie(
            "sessionId",
            sessionId,
            "/")
        {
            HttpOnly = true,
            Secure = Context.Request.IsSecureConnection,
            Expires = DateTime.UtcNow.AddMinutes(30)
        };
        Context.Response.SetCookie(cookie);
        
        return Redirect(user.IsAdmin ? "/admin" : "/");
    }
}