using MyHttpServer.ViewModels;

namespace MyHttpServer.Services;

public class CardFilter
{
    private const int MaxQueryLength = 60;

    public string Query { get; init; } = string.Empty;
    public string Kind { get; init; } = string.Empty;       // "", character, drink, binge
    public string Property { get; init; } = string.Empty;   // "", action, sometimes, always, gamble, cheat
    public int CharacterId { get; init; }

    public bool IsActive =>
        Query.Length > 0 || Kind.Length > 0 || Property.Length > 0 || CharacterId != 0;

    public static CardFilter FromQuery(string? query, string? kind, string? property, string? character)
    {
        string text = (query ?? string.Empty).Trim();
        if (text.Length > MaxQueryLength)
            text = text[..MaxQueryLength];

        int.TryParse(character, out int characterId);

        return new CardFilter
        {
            Query = text,
            Kind = KnownOrEmpty(kind, CardKind.All),
            Property = KnownOrEmpty(property, CardProperty.All),
            CharacterId = characterId > 0 ? characterId : 0,
        };
    }

    private static string KnownOrEmpty(string? value, string[] known)
    {
        string normalized = (value ?? string.Empty).Trim().ToLowerInvariant();
        return known.Contains(normalized) ? normalized : string.Empty;
    }
}
