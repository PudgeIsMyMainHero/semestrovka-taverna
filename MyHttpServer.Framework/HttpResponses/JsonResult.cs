using System.Net;
using System.Text;
using System.Text.Json;

namespace MyHttpServer.Framework.HttpResponses;

public class JsonResult : IHttpResponseTypeResult
{
    private readonly object _data;
 
    public JsonResult(object data)
    {
        _data = data;
    }
 
    public async Task ExecuteAsync(
        HttpListenerResponse response,
        CancellationToken cancellationToken = default)
    {
        byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(
            _data);
 
        response.StatusCode = 200;
        response.ContentType =
            "application/json; charset=utf-8";
        response.ContentLength64 = buffer.Length;
 
        await response.OutputStream.WriteAsync(
            buffer,
            cancellationToken);
    }
}