using System.Net;
using System.Text;
using MiniTemplateEngine;

namespace MyHttpServer.Framework.HttpResponses;

public class PageResult : IHttpResponseTypeResult
{
    private readonly string _templatePath;
    private readonly object _model;

    public PageResult(string templatePath, object model)
    {
        _templatePath = templatePath;
        _model = model;
    }
    public async Task ExecuteAsync(HttpListenerResponse response, CancellationToken cancellationToken = default)
    {
        var renderer = new HtmlTemplateRenderer();

        string html = renderer.RenderFromFile(
            _templatePath,
            _model);

        byte[] buffer = Encoding.UTF8.GetBytes(html);

        response.StatusCode = 200;
        response.ContentType = "text/html; charset=utf-8";
        response.ContentLength64 = buffer.Length;

        await response.OutputStream.WriteAsync(
            buffer,
            cancellationToken);
    }
}