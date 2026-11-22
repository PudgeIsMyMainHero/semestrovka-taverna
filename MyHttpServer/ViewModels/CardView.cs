namespace MyHttpServer.ViewModels;

// Тип карты 
public static class CardKind
{
    public const string Character = "character";
    public const string Drink = "drink";
    public const string Binge = "binge";

    public static readonly string[] All = { Character, Drink, Binge };
}

// Свойство карты
public static class CardProperty
{
    public const string Action = "action";
    public const string Sometimes = "sometimes";
    public const string Always = "always";
    public const string Gamble = "gamble";
    public const string Cheat = "cheat";

    public static readonly string[] All = { Action, Sometimes, Always, Gamble, Cheat };
}


// Представление карточки 
public class CardView
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Kind { get; set; } = string.Empty;
    public string KindLabel { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public int CharacterId { get; set; }

    public bool IsAction { get; set; }
    public bool IsSometimes { get; set; }
    public bool IsAlways { get; set; }
    public bool IsGamble { get; set; }
    public bool IsCheat { get; set; }

    public bool IsDrink { get; set; }
    public int Strength { get; set; }
    public bool HasChaser { get; set; }
    public bool HasFortitudeChange { get; set; }
    public string FortitudeText { get; set; } = string.Empty;

    public bool HasBadges { get; set; }
    public int Copies { get; set; } = 1;
    public bool HasCopies { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool HasImage { get; set; }
    public string SearchText { get; set; } = string.Empty;
}
