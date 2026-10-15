namespace RaceDay.Api.Models;

public class Result
{
    public int Id { get; set; }

    // Foreign key (1-to-1 with Enrolment)
    public int EnrolmentId { get; set; }
    public Enrolment Enrolment { get; set; } = null!;

    public TimeSpan FinishTime { get; set; }
    public int Position { get; set; }
}