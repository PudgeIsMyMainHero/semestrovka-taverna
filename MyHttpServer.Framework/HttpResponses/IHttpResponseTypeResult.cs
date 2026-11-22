using System.Net;

namespace MyHttpServer.Framework.HttpResponses;

public interface IHttpResponseTypeResult
{ 
        Task ExecuteAsync(HttpListenerResponse response,
        CancellationToken cancellationToken = default);
}