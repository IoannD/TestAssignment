using System.Text.Json.Serialization;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Application.Dtos;

public class CreateClinicalTrialDto
{
    [JsonPropertyName("trialId")] public string TrialId { get; init; }

    [JsonPropertyName("title")] public string Title { get; init; }

    [JsonPropertyName("startDate")] public DateTime? StartDate { get; init; }

    [JsonPropertyName("endDate")] public DateTime? EndDate { get; init; }

    [JsonPropertyName("participants")] public int Participants { get; init; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; init; }
}