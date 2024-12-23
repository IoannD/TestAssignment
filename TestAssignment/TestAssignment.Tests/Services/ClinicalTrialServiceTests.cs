using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using TestAssignment.Application.Dtos;
using TestAssignment.Application.MappingProfile;
using TestAssignment.Application.Services;
using TestAssignment.Domain.AbstractRepositories;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Tests.Services;

public class ClinicalTrialServiceTests
{
    private readonly IClinicalTrialService _clinicalTrialService;
    private readonly IJsonConversionService _jsonConversionService;
    private readonly IRepository<ClinicalTrial> _repository;

    public ClinicalTrialServiceTests()
    {
        _repository = Substitute.For<IRepository<ClinicalTrial>>();
        _jsonConversionService = Substitute.For<IJsonConversionService>();

        var mapperConfiguration = new MapperConfiguration(config => config.AddProfile<ApplicationMappingProfile>());
        var mapper = new Mapper(mapperConfiguration);

        _clinicalTrialService = new ClinicalTrialService(new NullLogger<ClinicalTrialService>(),
            _jsonConversionService, _repository, mapper);
    }

    [Fact]
    public async Task ShouldSetEndDateIfNullAndOngoingStatus()
    {
        var createClinicalTrialDto = new CreateClinicalTrialDto { StartDate = DateTime.Now, Status = Status.Ongoing };
        var expectedEndDate = createClinicalTrialDto.StartDate.Value.AddMonths(1);
        _jsonConversionService.Convert(Arg.Any<string>()).Returns(createClinicalTrialDto);

        await _clinicalTrialService.CreateFromFileAsync(string.Empty);

        await _repository.Received(1).CreateAsync(Arg.Is<ClinicalTrial>(x => x.EndDate == expectedEndDate));
    }

    [Fact]
    public async Task ShouldNotSetDurationInDaysWhenEndDateIsNull()
    {
        _jsonConversionService.Convert(Arg.Any<string>()).Returns(
            new CreateClinicalTrialDto
            {
                Status = Status.NotStarted,
                EndDate = null
            }
        );

        await _clinicalTrialService.CreateFromFileAsync(string.Empty);

        await _repository.Received(1).CreateAsync(Arg.Is<ClinicalTrial>(x => x.EndDate == null));
    }

    [Theory]
    [InlineData(Status.Completed)]
    [InlineData(Status.NotStarted)]
    public async Task ShouldSetDurationInDaysWhenEndDateNotNull(Status status)
    {
        var startDate = DateTime.Now;
        var durationInDays = 2;
        _jsonConversionService.Convert(Arg.Any<string>()).Returns(
            new CreateClinicalTrialDto
            {
                Status = status,
                StartDate = startDate,
                EndDate = startDate.AddDays(durationInDays - 1)
            }
        );

        await _clinicalTrialService.CreateFromFileAsync(string.Empty);

        await _repository.Received(1).CreateAsync(Arg.Is<ClinicalTrial>(x => x.DurationInDays == durationInDays));
    }
}