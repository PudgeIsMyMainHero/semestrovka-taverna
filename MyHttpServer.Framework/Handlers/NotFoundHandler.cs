using System.Net;

namespace MyHttpServer.Framework.Handlers;

public class NotFoundHandler : Handler
{
    public override async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        response.StatusCode = 404;
        response.ContentType = "text/html";
        var filepath = Directory.GetCurrentDirectory() + "/static/404.html";
        byte[] buffer = await File.ReadAllBytesAsync(filepath);

        response.ContentLength64 = buffer.Length;

        using Stream output = response.OutputStream;
        await output.WriteAsync(buffer);
        await output.FlushAsync();
        response.Close();
    }
}