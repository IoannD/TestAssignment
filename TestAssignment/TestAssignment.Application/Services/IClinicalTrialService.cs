using TestAssignment.Application.Dtos;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Application.Services;

public interface IClinicalTrialService
{
    Task CreateFromFileAsync(string jsonData, CancellationToken cancellationToken = default);
    Task<ClinicalTrialDto?> GetAsync(long id, CancellationToken cancellationToken = default);

    Task<List<ClinicalTrialDto>> GetAsync(Status? status, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default);
}