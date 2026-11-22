using MyHttpServer.Entities;
using MyHttpServer.Framework;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.HttpResponses;
using MyHttpServer.Services;
using ORM;

namespace MyHttpServer.Controllers;


[HttpController("characters")]
public class CharactersController : ControllerBase
{
    [Get("")]
    public async Task<IHttpResponseTypeResult> Index()
    {
        var orm = new OrmContext(ConnectionString);
        var characters = await orm.GetAllAsync<Character>("characters");

        return Page("Templates/characters.html", new { Characters = characters });
    }
    [Get("details")]
    public async Task<IHttpResponseTypeResult> Details([FromQuery] int id)
    {
        var orm = new OrmContext(ConnectionString);
        var characters = await orm.GetAllAsync<Character>("characters");

        var character = characters.FirstOrDefault(c => c.Id == id);
        if (character is null) return null!;

        var others = characters.Where(c => c.Id != id).ToList();

        var catalog = new CatalogService(orm);
        var deck = await catalog.GetDeckAsync(character);

        return Page("Templates/character.html", new
        {
            Character = character,
            Others = others,
            Deck = deck,
            DeckCount = deck.Count,
        });
    }
}
