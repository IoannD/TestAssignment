using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TestAssignment.Application.Dtos;
using TestAssignment.Domain.AbstractRepositories;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Application.Services;

internal class ClinicalTrialService : IClinicalTrialService
{
    private readonly IJsonConversionService _jsonConversionService;
    private readonly ILogger<ClinicalTrialService> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<ClinicalTrial> _repository;

    public ClinicalTrialService(ILogger<ClinicalTrialService> logger,
        IJsonConversionService jsonConversionService, IRepository<ClinicalTrial> repository,
        IMapper mapper)
    {
        _logger = logger;
        _jsonConversionService = jsonConversionService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task CreateFromFileAsync(string jsonData, CancellationToken cancellationToken = default)
    {
        var clinicalTrial = _mapper.Map<ClinicalTrial>(_jsonConversionService.Convert(jsonData));

        if (clinicalTrial.EndDate == null && clinicalTrial.Status == Status.Ongoing)
            clinicalTrial.EndDate = clinicalTrial.StartDate.AddMonths(1);

        clinicalTrial.DurationInDays = clinicalTrial.EndDate != null
            ? (clinicalTrial.EndDate.Value.Date - clinicalTrial.StartDate.Date).Days + 1
            : 0;

        await _repository.CreateAsync(clinicalTrial, cancellationToken);
    }

    public async Task<ClinicalTrialDto?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var clinicalTrial = await _repository.GetOneAsync(x => x.Id == id, cancellationToken);

        var clinicalTrialDto = _mapper.Map<ClinicalTrialDto>(clinicalTrial);

        if (clinicalTrialDto == null)
            _logger.LogError("ClinicalTrial not found (id={})", id);

        return clinicalTrialDto;
    }

    public async Task<List<ClinicalTrialDto>> GetAsync(Status? status,
        int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        Expression<Func<ClinicalTrial, bool>> predicate = x => true;
        if (status != null)
            predicate = x => x.Status == status;

        var clinicalTrials = await _repository.GetAsync(predicate, pageNumber, pageSize, cancellationToken);

        return _mapper.Map<List<ClinicalTrialDto>>(clinicalTrials);
    }
}