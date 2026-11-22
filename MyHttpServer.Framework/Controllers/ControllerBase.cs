using System.Net;
using MyHttpServer.Framework.Authentication;
using MyHttpServer.Framework.HttpResponses;

namespace MyHttpServer.Framework;

public class ControllerBase
{
    protected HttpListenerContext Context { get; private set; }
    protected string ConnectionString { get; private set; }
    
    protected IUserSessionService SessionService
    {
        get;
        private set;
    } = null!;
    
    public void Initialize(
        HttpListenerContext context,
        IUserSessionService sessionService, string connectionString)
    {
        Context = context;
        SessionService = sessionService;
        ConnectionString = connectionString;
    }

     protected JsonResult Json(object obj) => new JsonResult(obj);
     protected HtmlResult Html(string html) => new HtmlResult(html);
     
     protected PageResult Page(string path,  object obj) => new PageResult(path, obj);
     
     protected RedirectResult Redirect(string url) => new RedirectResult(url);
}
