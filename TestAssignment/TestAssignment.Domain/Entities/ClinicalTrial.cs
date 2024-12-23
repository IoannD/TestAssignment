namespace TestAssignment.Domain.Entities;

public class ClinicalTrial
{
    public long Id { get; set; }
    public string TrialId { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Participants { get; set; }
    public Status Status { get; set; }
    public int DurationInDays { get; set; }
}