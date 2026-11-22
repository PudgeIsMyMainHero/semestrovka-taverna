namespace MyHttpServer.Entities;


public class Character
{
    public int Id { get; set; }
    public string PortraitPath { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Pros { get; set; } = string.Empty;
    public string Cons { get; set; } = string.Empty;

}
