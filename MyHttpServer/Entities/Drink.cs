namespace MyHttpServer.Entities;

public class Drink
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public bool IsBinge { get; set; }

    public int Strength { get; set; }

    public bool HasChaser { get; set; }

    public int FortitudeChange { get; set; }

    public int Copies { get; set; } = 1;

    public string? ImagePath { get; set; }
}
