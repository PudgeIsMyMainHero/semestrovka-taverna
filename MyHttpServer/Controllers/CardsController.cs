using MyHttpServer.Framework;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.HttpResponses;
using MyHttpServer.Services;
using MyHttpServer.ViewModels;
using ORM;

namespace MyHttpServer.Controllers;


[HttpController("cards")]
public class CardsController : ControllerBase
{
    [Get("")]
    public async Task<PageResult> Index(
        [FromQuery] string q = "",
        [FromQuery] string kind = "",
        [FromQuery] string property = "",
        [FromQuery] string character = "")
    {
        var catalog = new CatalogService(new OrmContext(ConnectionString));

        var filter = CardFilter.FromQuery(q, kind, property, character);
        var result = await catalog.SearchAsync(filter);

        return Page("Templates/cards.html", CardsPageModel.Create(result, filter));
    }
}
