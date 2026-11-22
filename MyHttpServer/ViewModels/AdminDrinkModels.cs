using System.Net;
using MyHttpServer.Entities;

namespace MyHttpServer.ViewModels;

// Строка таблицы /admin/drinks.
public class AdminDrinkRow
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string KindLabel { get; set; } = string.Empty;   // «Выпивка» или «Попойка»
    public int Strength { get; set; }
    public bool HasImage { get; set; }

    public static AdminDrinkRow From(Drink drink) => new()
    {
        Id = drink.Id,
        Name = WebUtility.HtmlEncode(drink.Name),
        KindLabel = drink.IsBinge ? "Пьянка" : "Выпивка",
        Strength = drink.Strength,
        HasImage = !string.IsNullOrWhiteSpace(drink.ImagePath),
    };
}

// Модель списка /admin/drinks
public class AdminDrinksListModel
{
    public List<AdminDrinkRow> Rows { get; set; } = new();
    public int Total { get; set; }
}

// Модель формы создания/редактирования выпивки (/admin/drinks/new и /admin/drinks/edit).

public class AdminDrinkFormModel
{
    public bool IsEdit { get; set; }
    public int Id { get; set; }
    public string FormAction { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string SubmitLabel { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Strength { get; set; }
    public int FortitudeChange { get; set; }
    public int Copies { get; set; } = 1;
    public string ImagePath { get; set; } = string.Empty;

    public string ChkBinge { get; set; } = string.Empty;
    public string ChkChaser { get; set; } = string.Empty;

    public List<OptionView> ImageOptions { get; set; } = new();

    public bool HasErrors { get; set; }
    public List<string> Errors { get; set; } = new();
}
