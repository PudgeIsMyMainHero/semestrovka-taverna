using System.Net;

namespace MyHttpServer.Framework.Handlers;

public class StaticFileHandler : Handler
{
    public override async Task HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        string path = request.Url.LocalPath;
        bool isFile = path.Contains('.');

        if (isFile)
        {
            try
            {
                string filePath = FindFile(path);
                if (filePath is null)
                {
                    // Передача запроса дальше по цепи
                    if (Successor != null)
                    {
                        await Successor.HandleRequest(context);
                    }

                    return;
                }
                
                FileInfo fileInfo = new FileInfo(filePath);
                response.ContentType =
                    ContentTypes.GetContentType(fileInfo.Extension);
                response.StatusCode = 200;

                byte[] buffer = await File.ReadAllBytesAsync(filePath);

                response.ContentLength64 = buffer.Length;

                await response.OutputStream.WriteAsync(buffer);
                response.Close();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Передача запроса дальше по цепи
        if (Successor != null)
        {
            await Successor.HandleRequest(context);
        }
    }

    private string? FindFile(string path)
    {
        string staticDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "static");

        string relativePath = path.TrimStart('/');

        string filePath = Path.Combine(
            staticDirectory,
            relativePath);

        if (File.Exists(filePath))
        {
            return filePath;
        }

        int separatorIndex = relativePath.IndexOf('/');

        if (separatorIndex >= 0)
        {
            string fallbackPath = Path.Combine(
                staticDirectory,
                relativePath.Substring(separatorIndex + 1));

            if (File.Exists(fallbackPath))
            {
                return fallbackPath;
            }
        }

        return null;
    }
}