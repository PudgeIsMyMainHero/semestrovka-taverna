namespace MyHttpServer.Entities;


public class Card
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public bool IsAction { get; set; }
    public bool IsSometimes { get; set; }
    public bool IsAlways { get; set; }
    public bool IsGamble { get; set; }
    public bool IsCheat { get; set; }

    public int Copies { get; set; } = 1;

    public string? ImagePath { get; set; }
}
