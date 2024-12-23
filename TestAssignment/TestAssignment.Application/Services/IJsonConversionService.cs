using TestAssignment.Application.Dtos;

namespace TestAssignment.Application.Services;

public interface IJsonConversionService
{
    CreateClinicalTrialDto Convert(string jsonData);
}