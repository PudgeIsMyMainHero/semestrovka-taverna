using System.Text.Json;
using MyHttpServer.Framework;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.HttpResponses;

namespace MyHttpServer.Controllers;

[HttpController("rules")]
public class RulesController : ControllerBase
{
    [Get("")]
    public async Task<IHttpResponseTypeResult> Rules()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "static", "rules.html");
        if (File.Exists(path))
        {
            string html = await File.ReadAllTextAsync(path);
            return Html(html);
        }

        return null!;
    }
}