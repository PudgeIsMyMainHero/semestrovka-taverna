using System.Text.Json;

namespace MyHttpServer;

public static class ContentTypes
{
    private static readonly Dictionary<string, string> _contentTypes = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("mime-types.json"));

    public static string GetContentType(string fileExtension)
    {
        return _contentTypes[fileExtension.ToLower()];
    }
}