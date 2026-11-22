using MyHttpServer.Entities;
using MyHttpServer.Framework;
using MyHttpServer.Framework.Attributes;
using MyHttpServer.Framework.HttpResponses;
using MyHttpServer.Services;
using MyHttpServer.ViewModels;
using ORM;

namespace MyHttpServer.Controllers;

[HttpController("admin")]
[RequireAdmin]
public class AdminController : ControllerBase
{
    private OrmContext Orm => new(ConnectionString);
    
    
    [Get("")]
    public async Task<PageResult> Dashboard()
    {
        var orm = Orm;
        var characters = await orm.GetAllAsync<Character>("characters");
        var cards = await orm.GetAllAsync<Card>("cards");
        var drinks = await orm.GetAllAsync<Drink>("drinks");

        return Page("Templates/admin/dashboard.html", new
        {
            CharacterCount = characters.Count,
            CardCount = cards.Count,
            DrinkCount = drinks.Count,
            MissingImages = cards.Count(c => string.IsNullOrWhiteSpace(c.ImagePath))
                           + drinks.Count(d => string.IsNullOrWhiteSpace(d.ImagePath)),
        });
    }



    [Get("cards")]
    public async Task<PageResult> CardsList()
    {
        var orm = Orm;
        var characters = await orm.GetAllAsync<Character>("characters");
        var cards = await orm.GetAllAsync<Card>("cards");
        var names = characters.ToDictionary(c => c.Id, c => c.Name);

        var rows = cards
            .OrderBy(c => names.GetValueOrDefault(c.CharacterId, ""), StringComparer.OrdinalIgnoreCase)
            .ThenBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .Select(c => AdminCardRow.From(c, names.GetValueOrDefault(c.CharacterId, "?")))
            .ToList();

        return Page("Templates/admin/cards-list.html", new AdminCardsListModel { Rows = rows, Total = rows.Count });
    }



    [Get("cards/new")]
    public async Task<PageResult> CardNew([FromQuery] int character = 0)
    {
        var model = await BuildCardForm(isEdit: false, id: 0, characterId: character);
        return Page("Templates/admin/card-form.html", model);
    }

    [Post("cards/create")]
    public async Task<IHttpResponseTypeResult> CardCreate(
        [FromForm] int characterid = 0,
        [FromForm] string name = "",
        [FromForm] string description = "",
        [FromForm] bool isaction = false,
        [FromForm] bool issometimes = false,
        [FromForm] bool isalways = false,
        [FromForm] bool isgamble = false,
        [FromForm] bool ischeat = false,
        [FromForm] int copies = 1,
        [FromForm] string imagepath = "")
    {
        var card = new Card
        {
            CharacterId = characterid,
            Name = name.Trim(),
            Description = description.Trim(),
            IsAction = isaction,
            IsSometimes = issometimes,
            IsAlways = isalways,
            IsGamble = isgamble,
            IsCheat = ischeat,
            Copies = copies < 1 ? 1 : copies,
            ImagePath = string.IsNullOrWhiteSpace(imagepath) ? null : imagepath,
        };

        var errors = ValidateCard(card);
        if (errors.Count > 0)
        {
            var form = await BuildCardForm(isEdit: false, id: 0, characterId: characterid, overrideCard: card);
            form.Errors = errors;
            form.HasErrors = true;
            return Page("Templates/admin/card-form.html", form);
        }

        await Orm.InsertAsync("cards", card);
        return Redirect("/admin/cards");
    }


    [Get("cards/edit")]
    public async Task<IHttpResponseTypeResult> CardEdit([FromQuery] int id)
    {
        var orm = Orm;
        var cards = await orm.GetAllAsync<Card>("cards");
        var card = cards.FirstOrDefault(c => c.Id == id);
        if (card is null)
            return null!;

        var model = await BuildCardForm(isEdit: true, id: id, characterId: card.CharacterId, overrideCard: card);
        return Page("Templates/admin/card-form.html", model);
    }

    [Post("cards/update")]
    public async Task<IHttpResponseTypeResult> CardUpdate(
        [FromQuery] int id,
        [FromForm] int characterid = 0,
        [FromForm] string name = "",
        [FromForm] string description = "",
        [FromForm] bool isaction = false,
        [FromForm] bool issometimes = false,
        [FromForm] bool isalways = false,
        [FromForm] bool isgamble = false,
        [FromForm] bool ischeat = false,
        [FromForm] int copies = 1,
        [FromForm] string imagepath = "")
    {
        var card = new Card
        {
            Id = id,
            CharacterId = characterid,
            Name = name.Trim(),
            Description = description.Trim(),
            IsAction = isaction,
            IsSometimes = issometimes,
            IsAlways = isalways,
            IsGamble = isgamble,
            IsCheat = ischeat,
            Copies = copies < 1 ? 1 : copies,
            ImagePath = string.IsNullOrWhiteSpace(imagepath) ? null : imagepath,
        };

        var errors = ValidateCard(card);
        if (errors.Count > 0)
        {
            var form = await BuildCardForm(isEdit: true, id: id, characterId: characterid, overrideCard: card);
            form.Errors = errors;
            form.HasErrors = true;
            return Page("Templates/admin/card-form.html", form);
        }

        await Orm.UpdateAsync("cards", id, card);
        return Redirect("/admin/cards");
    }

    [Post("cards/delete")]
    public async Task<IHttpResponseTypeResult> CardDelete([FromQuery] int id)
    {
        await Orm.DeleteAsync("cards", id);
        return Redirect("/admin/cards");
    }


    [Get("drinks")]
    public async Task<PageResult> DrinksList()
    {
        var drinks = await Orm.GetAllAsync<Drink>("drinks");
        var rows = drinks
            .OrderBy(d => d.IsBinge)
            .ThenBy(d => d.Name, StringComparer.OrdinalIgnoreCase)
            .Select(AdminDrinkRow.From)
            .ToList();

        return Page("Templates/admin/drinks-list.html", new AdminDrinksListModel { Rows = rows, Total = rows.Count });
    }
    

    [Get("drinks/new")]
    public async Task<PageResult> DrinkNew()
    {
        var model = await BuildDrinkForm(isEdit: false, id: 0);
        return Page("Templates/admin/drink-form.html", model);
    }

    [Post("drinks/create")]
    public async Task<IHttpResponseTypeResult> DrinkCreate(
        [FromForm] string name = "",
        [FromForm] string description = "",
        [FromForm] bool isbinge = false,
        [FromForm] int strength = 0,
        [FromForm] bool haschaser = false,
        [FromForm] int fortitudechange = 0,
        [FromForm] int copies = 1,
        [FromForm] string imagepath = "")
    {
        var drink = new Drink
        {
            Name = name.Trim(),
            Description = description.Trim(),
            IsBinge = isbinge,
            Strength = strength,
            HasChaser = haschaser,
            FortitudeChange = fortitudechange,
            Copies = copies < 1 ? 1 : copies,
            ImagePath = string.IsNullOrWhiteSpace(imagepath) ? null : imagepath,
        };

        var errors = ValidateDrink(drink);
        if (errors.Count > 0)
        {
            var form = await BuildDrinkForm(isEdit: false, id: 0, overrideDrink: drink);
            form.Errors = errors;
            form.HasErrors = true;
            return Page("Templates/admin/drink-form.html", form);
        }

        await Orm.InsertAsync("drinks", drink);
        return Redirect("/admin/drinks");
    }

    [Get("drinks/edit")]
    public async Task<IHttpResponseTypeResult> DrinkEdit([FromQuery] int id)
    {
        var drinks = await Orm.GetAllAsync<Drink>("drinks");
        var drink = drinks.FirstOrDefault(d => d.Id == id);
        if (drink is null)
            return null!;

        var model = await BuildDrinkForm(true, id, drink);
        return Page("Templates/admin/drink-form.html", model);
    }

    [Post("drinks/update")]
    public async Task<IHttpResponseTypeResult> DrinkUpdate(
        [FromQuery] int id,
        [FromForm] string name = "",
        [FromForm] string description = "",
        [FromForm] bool isbinge = false,
        [FromForm] int strength = 0,
        [FromForm] bool haschaser = false,
        [FromForm] int fortitudechange = 0,
        [FromForm] int copies = 1,
        [FromForm] string imagepath = "")
    {
        var drink = new Drink
        {
            Id = id,
            Name = name.Trim(),
            Description = description.Trim(),
            IsBinge = isbinge,
            Strength = strength,
            HasChaser = haschaser,
            FortitudeChange = fortitudechange,
            Copies = copies < 1 ? 1 : copies,
            ImagePath = string.IsNullOrWhiteSpace(imagepath) ? null : imagepath,
        };

        var errors = ValidateDrink(drink);
        if (errors.Count > 0)
        {
            var form = await BuildDrinkForm(true, id, drink);
            form.Errors = errors;
            form.HasErrors = true;
            return Page("Templates/admin/drink-form.html", form);
        }

        await Orm.UpdateAsync("drinks", id, drink);
        return Redirect("/admin/drinks");
    }

    [Post("drinks/delete")]
    public async Task<IHttpResponseTypeResult> DrinkDelete([FromQuery] int id)
    {
        await Orm.DeleteAsync("drinks", id);
        return Redirect("/admin/drinks");
    }

 

    private static List<string> ValidateCard(Card card)
    {
        var errors = new List<string>();
        if (card.CharacterId <= 0) errors.Add("Выберите персонажа.");
        if (card.Name.Length == 0) errors.Add("Укажите название карты.");
        if (card.Name.Length > 80) errors.Add("Название слишком длинное (максимум 80 символов).");
        if (card.Description.Length == 0) errors.Add("Укажите текст карты.");
        if (!(card.IsAction || card.IsSometimes || card.IsAlways || card.IsGamble || card.IsCheat))
            errors.Add("Отметьте хотя бы одно свойство карты.");
        return errors;
    }

    private static List<string> ValidateDrink(Drink drink)
    {
        var errors = new List<string>();
        if (drink.Name.Length == 0) errors.Add("Укажите название.");
        if (drink.Name.Length > 80) errors.Add("Название слишком длинное (максимум 80 символов).");
        if (drink.Description.Length == 0) errors.Add("Укажите описание эффекта.");
        return errors;
    }

    private async Task<AdminCardFormModel> BuildCardForm(bool isEdit, int id, int characterId, Card? overrideCard = null)
    {
        var orm = Orm;
        var characters = await orm.GetAllAsync<Character>("characters");
        var cards = await orm.GetAllAsync<Card>("cards");

        var card = overrideCard ?? new Card { CharacterId = characterId, Copies = 1 };

        var images = AdminImageScanner.ListImages("cards");
        var usedBy = cards
            .Where(c => c.Id != id && !string.IsNullOrWhiteSpace(c.ImagePath))
            .ToDictionary(c => c.ImagePath!, c => c.Name, StringComparer.OrdinalIgnoreCase);

        var imageOptions = new List<OptionView> { new() { Value = "", Label = "Без картинки", Selected = card.ImagePath is null ? "selected" : "" } };
        imageOptions.AddRange(images.Select(path => new OptionView
        {
            Value = path,
            Label = usedBy.TryGetValue(path, out var owner) ? $"{path} (занято: «{owner}»)" : path,
            Selected = path.Equals(card.ImagePath, StringComparison.OrdinalIgnoreCase) ? "selected" : "",
        }));

        var characterOptions = characters
            .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .Select(c => new OptionView
            {
                Value = c.Id.ToString(),
                Label = c.Name,
                Selected = c.Id == card.CharacterId ? "selected" : "",
            })
            .ToList();

        return new AdminCardFormModel
        {
            IsEdit = isEdit,
            Id = id,
            FormAction = isEdit ? $"/admin/cards/update?id={id}" : "/admin/cards/create",
            Title = isEdit ? "Редактирование карты" : "Новая карта",
            SubmitLabel = isEdit ? "Сохранить" : "Добавить карту",
            Name = card.Name,
            Description = card.Description,
            Copies = card.Copies,
            ImagePath = card.ImagePath ?? "",
            ChkAction = card.IsAction ? "checked" : "",
            ChkSometimes = card.IsSometimes ? "checked" : "",
            ChkAlways = card.IsAlways ? "checked" : "",
            ChkGamble = card.IsGamble ? "checked" : "",
            ChkCheat = card.IsCheat ? "checked" : "",
            CharacterOptions = characterOptions,
            ImageOptions = imageOptions,
        };
    }

    private async Task<AdminDrinkFormModel> BuildDrinkForm(bool isEdit, int id, Drink? overrideDrink = null)
    {
        var drinks = await Orm.GetAllAsync<Drink>("drinks");
        var drink = overrideDrink ?? new Drink { Copies = 1 };

        var images = AdminImageScanner.ListImages("drinks");
        var usedBy = drinks
            .Where(d => d.Id != id && !string.IsNullOrWhiteSpace(d.ImagePath))
            .ToDictionary(d => d.ImagePath!, d => d.Name, StringComparer.OrdinalIgnoreCase);

        var imageOptions = new List<OptionView> { new() { Value = "", Label = "Без картинки", Selected = drink.ImagePath is null ? "selected" : "" } };
        imageOptions.AddRange(images.Select(path => new OptionView
        {
            Value = path,
            Label = usedBy.TryGetValue(path, out var owner) ? $"{path} (занято: «{owner}»)" : path,
            Selected = path.Equals(drink.ImagePath, StringComparison.OrdinalIgnoreCase) ? "selected" : "",
        }));

        return new AdminDrinkFormModel
        {
            IsEdit = isEdit,
            Id = id,
            FormAction = isEdit ? $"/admin/drinks/update?id={id}" : "/admin/drinks/create",
            Title = isEdit ? "Редактирование выпивки" : "Новая выпивка",
            SubmitLabel = isEdit ? "Сохранить" : "Добавить выпивку",
            Name = drink.Name,
            Description = drink.Description,
            Strength = drink.Strength,
            FortitudeChange = drink.FortitudeChange,
            Copies = drink.Copies,
            ImagePath = drink.ImagePath ?? "",
            ChkBinge = drink.IsBinge ? "checked" : "",
            ChkChaser = drink.HasChaser ? "checked" : "",
            ImageOptions = imageOptions,
        };
    }
}
