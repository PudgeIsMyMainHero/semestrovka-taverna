using MyHttpServer.Entities;
using MyHttpServer.Framework;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.HttpResponses;
using ORM;

namespace MyHttpServer.Controllers;


[HttpController("")]
public class HomeController : ControllerBase
{
    [Get("")]
    public async Task<IHttpResponseTypeResult> Index()
    {
        var orm = new OrmContext(ConnectionString);
        var characters = await orm.GetAllAsync<Character>("characters");

        return Page("Templates/index.html", new { Characters = characters });
    }
}
