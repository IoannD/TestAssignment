using TestAssignment.Domain.Entities;

namespace TestAssignment.Application.Dtos;

public class ClinicalTrialDto
{
    public long Id { get; init; }
    public string TrialId { get; init; }
    public string Title { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int Participants { get; init; }
    public Status Status { get; init; }
    public int DurationInDays { get; init; }
}