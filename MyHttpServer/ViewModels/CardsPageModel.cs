using System.Net;
using MyHttpServer.Services;

namespace MyHttpServer.ViewModels;

// Для выпадающего списка
public class OptionView
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Selected { get; set; } = string.Empty;
}

//Модель страницы /cards
public class CardsPageModel
{
    public List<CardView> Cards { get; set; } = new();
    public int Found { get; set; }
    public int Total { get; set; }
    public bool IsFiltered { get; set; }

    public string Query { get; set; } = string.Empty;

    public List<OptionView> Kinds { get; set; } = new();
    public List<OptionView> Properties { get; set; } = new();
    public List<OptionView> Characters { get; set; } = new();

    public static CardsPageModel Create(CatalogResult result, CardFilter filter)
    {
        var characters = new List<(string Value, string Label)> { ("", "Любой") };
        characters.AddRange(result.Characters.Select(c => (c.Id.ToString(), WebUtility.HtmlEncode(c.Name))));

        return new CardsPageModel
        {
            Cards = result.Cards,
            Found = result.Cards.Count,
            Total = result.Total,
            IsFiltered = filter.IsActive,
            Query = WebUtility.HtmlEncode(filter.Query),
            Kinds = Options(filter.Kind, new[]
            {
                ("", "Любой"),
                (CardKind.Character, "Карты персонажей"),
                (CardKind.Drink, "Выпивка"),
                (CardKind.Binge, "Пьянки"),
            }),
            Properties = Options(filter.Property, new[]
            {
                ("", "Любое"),
                (CardProperty.Action, "Действие"),
                (CardProperty.Sometimes, "Иногда"),
                (CardProperty.Always, "Всегда"),
                (CardProperty.Gamble, "Азарт"),
                (CardProperty.Cheat, "Обман"),
            }),
            Characters = Options(filter.CharacterId == 0 ? "" : filter.CharacterId.ToString(), characters),
        };
    }

    private static List<OptionView> Options(string selected, IEnumerable<(string Value, string Label)> items)
    {
        return items
            .Select(i => new OptionView
            {
                Value = i.Value,
                Label = i.Label,
                Selected = i.Value == selected ? "selected" : string.Empty,
            })
            .ToList();
    }
}
