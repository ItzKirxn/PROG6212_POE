namespace RaceDay.Api.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Foreign key
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
}