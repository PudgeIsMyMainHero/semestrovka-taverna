using System.Net;

namespace MyHttpServer.Framework.HttpResponses;

public class RedirectResult : IHttpResponseTypeResult
{
    private readonly string _location;

    public RedirectResult(string location)
    {
        _location = location;
    }

    public async Task ExecuteAsync(
        HttpListenerResponse response,
        CancellationToken cancellationToken = default)
    {
        response.StatusCode = 302;
        response.RedirectLocation = _location;   
        response.ContentLength64 = 0;

        await Task.CompletedTask;
    }
}
