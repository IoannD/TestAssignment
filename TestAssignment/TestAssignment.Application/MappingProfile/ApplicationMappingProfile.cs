using AutoMapper;
using TestAssignment.Application.Dtos;
using TestAssignment.Domain.Entities;

namespace TestAssignment.Application.MappingProfile;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<CreateClinicalTrialDto, ClinicalTrial>();

        CreateMap<ClinicalTrial, ClinicalTrialDto>();
    }
}