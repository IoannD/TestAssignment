using Microsoft.Extensions.DependencyInjection;
using TestAssignment.Application.MappingProfile;
using TestAssignment.Application.Services;

namespace TestAssignment.Application.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationMappingProfile));

        services.AddScoped<IJsonConversionService, JsonConversionService>();
        services.AddScoped<IClinicalTrialService, ClinicalTrialService>();

        return services;
    }
}