using System.Net;
using MyHttpServer.Entities;

namespace MyHttpServer.ViewModels;

//Строка таблицы /admin/cards
public class AdminCardRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CharacterName { get; set; } = string.Empty;
    public string PropertiesLabel { get; set; } = string.Empty;
    public int Copies { get; set; }
    public bool HasImage { get; set; }

    public static AdminCardRow From(Card card, string characterName)
    {
        var props = new List<string>();
        if (card.IsAction) props.Add("Действие");
        if (card.IsSometimes) props.Add("Иногда");
        if (card.IsAlways) props.Add("Всегда");
        if (card.IsGamble) props.Add("Азарт");
        if (card.IsCheat) props.Add("Обман");

        return new AdminCardRow
        {
            Id = card.Id,
            Name = WebUtility.HtmlEncode(card.Name),
            CharacterName = WebUtility.HtmlEncode(characterName),
            PropertiesLabel = props.Count > 0 ? WebUtility.HtmlEncode(string.Join(", ", props)) : "—",
            Copies = card.Copies,
            HasImage = !string.IsNullOrWhiteSpace(card.ImagePath),
        };
    }
}

// Модель списка /admin/cards
public class AdminCardsListModel
{
    public List<AdminCardRow> Rows { get; set; } = new();
    public int Total { get; set; }
}


// Модель формы создания/редактирования карты (/admin/cards/new и /admin/cards/edit).
public class AdminCardFormModel
{
    public bool IsEdit { get; set; }
    public int Id { get; set; }
    public string FormAction { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string SubmitLabel { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Copies { get; set; } = 1;
    public string ImagePath { get; set; } = string.Empty;

    public string ChkAction { get; set; } = string.Empty;
    public string ChkSometimes { get; set; } = string.Empty;
    public string ChkAlways { get; set; } = string.Empty;
    public string ChkGamble { get; set; } = string.Empty;
    public string ChkCheat { get; set; } = string.Empty;

    public List<OptionView> CharacterOptions { get; set; } = new();
    public List<OptionView> ImageOptions { get; set; } = new();

    public bool HasErrors { get; set; }
    public List<string> Errors { get; set; } = new();
}
