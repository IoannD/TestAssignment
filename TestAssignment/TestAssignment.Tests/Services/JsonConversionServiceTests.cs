using System.ComponentModel.DataAnnotations;
using Shouldly;
using TestAssignment.Application.Services;

namespace TestAssignment.Tests.Services;

public class JsonConversionServiceTests
{
    private readonly IJsonConversionService _jsonConversionService = new JsonConversionService();

    public static TheoryData<string> InvalidJsonTestData => new()
    {
        """
        {
            "title": "Clinical Trial for Vaccine",
            "startDate": "2024-01-01",
            "endDate": "2024-12-01",
            "participants": 255,
            "status": "Ongoing"
        }
        """,

        """
        {
            "trialId": "12345",
            "startDate": "2024-01-01",
            "endDate": "2024-12-01",
            "participants": 255,
            "status": "Ongoing"
        }
        """,

        """
        {
            "trialId": "12345",
            "title": "Clinical Trial for Vaccine",
            "endDate": "2024-12-01",
            "participants": 255,
            "status": "Ongoing"
        }
        """,

        """
        {
            "trialId": "12345",
            "title": "Clinical Trial for Vaccine",
            "startDate": "2024-01-01",
            "endDate": "2024-12-01",
            "participants": 255
        }
        """,

        """
        {
            "trialId": "12345",
            "title": "Clinical Trial for Vaccine",
            "startDate": "2024-01-01",
            "endDate": "2024-12-01",
            "participants": 255,
            "status": "unknown status"
        }
        """
    };

    [Theory]
    [MemberData(nameof(InvalidJsonTestData))]
    public void ShouldThrowIfJsonInvalid(string jsonData)
    {
        Should.Throw<ValidationException>(() => _jsonConversionService.Convert(jsonData));
    }
}