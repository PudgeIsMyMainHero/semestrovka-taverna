using System.Security.Cryptography;
using System.Text;

namespace MyHttpServer.Services;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        var sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public static bool Verify(string inputtedPassword, string hashedPassword)
    {
        return Hash(inputtedPassword).Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
    }
}