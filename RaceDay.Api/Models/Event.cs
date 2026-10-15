namespace RaceDay.Api.Models;

public enum EventType
{
    Run,
    Walk,
    Cycle
}

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Distance { get; set; }
    public EventType EventType { get; set; }

    // Foreign key
    public int OrganiserId { get; set; }
    public User Organiser { get; set; } = null!;

    // Navigation
    public List<Category> Categories { get; set; } = new();
    public List<Enrolment> Enrolments { get; set; } = new();
}