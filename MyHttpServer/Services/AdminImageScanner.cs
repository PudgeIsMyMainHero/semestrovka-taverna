namespace MyHttpServer.Services;


// сканит картинки из static для выпадающего меню картинок из static
public static class AdminImageScanner
{
    private static readonly string[] Extensions = { ".jpg" };
    public static List<string> ListImages(string subfolder)
    {
        string root = Path.Combine(Directory.GetCurrentDirectory(), "static", subfolder);
        if (!Directory.Exists(root))
            return new List<string>();

        return Directory.GetFiles(root, "*", SearchOption.AllDirectories)
            .Where(path => Extensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
            .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/'))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
