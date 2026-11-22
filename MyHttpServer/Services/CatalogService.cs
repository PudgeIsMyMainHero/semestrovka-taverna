using System.Net;
using MyHttpServer.Entities;
using MyHttpServer.ViewModels;
using ORM;

namespace MyHttpServer.Services;

// Персонажи для списка в форме, найденные карточки и общее число карт.
public record CatalogResult(IReadOnlyList<Character> Characters, List<CardView> Cards, int Total);

// Собирает единый каталог из персонажей, карточек и выпивки и фильтрует его.
public class CatalogService
{
    private readonly OrmContext _orm;

    public CatalogService(OrmContext orm)
    {
        _orm = orm;
    }

    public async Task<CatalogResult> SearchAsync(CardFilter filter, CancellationToken cancellationToken = default)
    {
        var characters = await _orm.GetAllAsync<Character>("characters", cancellationToken);
        var cards = await _orm.GetAllAsync<Card>("cards", cancellationToken);
        var drinks = await _orm.GetAllAsync<Drink>("drinks", cancellationToken);

        var all = BuildViews(characters, cards, drinks);
        var found = Filter(all, filter);

        return new CatalogResult(characters, found, all.Count);
    }

    // Колода персонажа
    public async Task<List<CardView>> GetDeckAsync(Character character, CancellationToken cancellationToken = default)
    {
        var cards = await _orm.GetAllAsync<Card>("cards", cancellationToken);
        var own = cards.Where(c => c.CharacterId == character.Id);
        return BuildViews(new[] { character }, own, Array.Empty<Drink>());
    }

    // Склеивает сущности
    public static List<CardView> BuildViews(
        IEnumerable<Character> characters,
        IEnumerable<Card> cards,
        IEnumerable<Drink> drinks)
    {
        var names = characters.ToDictionary(c => c.Id, c => c.Name);
        var views = new List<CardView>();

        var orderedCards = cards
            .OrderBy(c => names.GetValueOrDefault(c.CharacterId, string.Empty), StringComparer.OrdinalIgnoreCase)
            .ThenBy(c => c.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var card in orderedCards)
        {
            string owner = names.GetValueOrDefault(card.CharacterId, "Неизвестный персонаж");
            bool hasBadges = card.IsAction || card.IsSometimes || card.IsAlways || card.IsGamble || card.IsCheat;

            views.Add(new CardView
            {
                Name = WebUtility.HtmlEncode(card.Name),
                Description = WebUtility.HtmlEncode(card.Description),
                Kind = CardKind.Character,
                KindLabel = "Карта персонажа",
                Owner = WebUtility.HtmlEncode(owner),
                CharacterId = card.CharacterId,
                IsAction = card.IsAction,
                IsSometimes = card.IsSometimes,
                IsAlways = card.IsAlways,
                IsGamble = card.IsGamble,
                IsCheat = card.IsCheat,
                HasBadges = hasBadges,
                Copies = card.Copies,
                HasCopies = card.Copies > 1,
                ImageUrl = ImageUrl("cards", card.ImagePath),
                HasImage = !string.IsNullOrWhiteSpace(card.ImagePath),
                SearchText = (card.Name + " " + card.Description).ToLower(),
            });
        }

        var orderedDrinks = drinks
            .OrderBy(d => d.IsBinge)
            .ThenBy(d => d.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var drink in orderedDrinks)
        {
            bool isPlainDrink = !drink.IsBinge;
            bool hasFortitude = drink.FortitudeChange != 0;

            views.Add(new CardView
            {
                Name = WebUtility.HtmlEncode(drink.Name),
                Description = WebUtility.HtmlEncode(drink.Description),
                Kind = drink.IsBinge ? CardKind.Binge : CardKind.Drink,
                KindLabel = drink.IsBinge ? "Пьянка" : "Выпивка",
                Owner = "Общая колода",
                CharacterId = 0,
                IsDrink = isPlainDrink,
                Strength = drink.Strength,
                HasChaser = drink.HasChaser,
                HasFortitudeChange = hasFortitude,
                FortitudeText = drink.FortitudeChange > 0 ? "+" + drink.FortitudeChange : drink.FortitudeChange.ToString(),
                HasBadges = isPlainDrink || drink.HasChaser || hasFortitude,
                Copies = drink.Copies,
                HasCopies = drink.Copies > 1,
                ImageUrl = ImageUrl("drinks", drink.ImagePath),
                HasImage = !string.IsNullOrWhiteSpace(drink.ImagePath),
                SearchText = (drink.Name + " " + drink.Description).ToLowerInvariant(),
            });
        }

        return views;
    }
    
    // Собирает путь картинки из static/
    private static string ImageUrl(string folder, string? path)
        => string.IsNullOrWhiteSpace(path) ? string.Empty : $"/{folder}/{path.TrimStart('/')}";

    // Фильтрует карточки
    public static List<CardView> Filter(IEnumerable<CardView> cards, CardFilter filter)
    {
        string query = filter.Query.ToLowerInvariant();

        return cards
            .Where(c => filter.Kind.Length == 0 || c.Kind == filter.Kind)
            .Where(c => filter.CharacterId == 0 || c.CharacterId == filter.CharacterId)
            .Where(c => HasProperty(c, filter.Property))
            .Where(c => query.Length == 0 || c.SearchText.Contains(query))
            .ToList();
    }
    
    private static bool HasProperty(CardView card, string property) => property switch
    {
        CardProperty.Action => card.IsAction,
        CardProperty.Sometimes => card.IsSometimes,
        CardProperty.Always => card.IsAlways,
        CardProperty.Gamble => card.IsGamble,
        CardProperty.Cheat => card.IsCheat,
        _ => true,
    };
}
