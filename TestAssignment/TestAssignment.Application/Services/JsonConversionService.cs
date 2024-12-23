using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;
using TestAssignment.Application.Dtos;
using TestAssignment.Application.Utils;
using TestAssignment.Application.Validators;

namespace TestAssignment.Application.Services;

internal class JsonConversionService : IJsonConversionService
{
    public CreateClinicalTrialDto Convert(string jsonData)
    {
        ArgumentNullException.ThrowIfNull(jsonData);

        var schema = """
                     {
                       "$schema": "http://json-schema.org/draft-07/schema#",
                       "title": "ClinicalTrialMetadata",
                       "type": "object",
                       "properties": {
                         "trialId": {
                           "type": "string"
                         },
                         "title": {
                           "type": "string"
                         },
                         "startDate": {
                           "type": "string",
                           "format": "date"
                         },
                         "endDate": {
                           "type": "string",
                           "format": "date"
                         },
                         "participants": {
                           "type": "integer",
                           "minimum": 1
                         },
                         "status": {
                           "type": "string",
                           "enum": ["Not Started", "Ongoing", "Completed"]
                         }
                       },
                       "required": ["trialId", "title", "startDate", "status"],
                       "additionalProperties": false
                     }
                     """;

        var jsonSchema = JsonSchema.FromText(schema);
        var result = jsonSchema.Evaluate(JsonNode.Parse(jsonData));
        if (!result.IsValid)
            throw new ValidationException(result.Errors?.Any() ?? false
                ? string.Join("\n", result.Errors)
                : "The file does not conform to the specified JSON schema.");

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new UtcDateTimeConverter()
            }
        };

        var createClinicalTrialDto = JsonSerializer.Deserialize<CreateClinicalTrialDto>(jsonData, options);

        ArgumentNullException.ThrowIfNull(createClinicalTrialDto);

        var validationResult = new UploadClinicalTrialValidator().Validate(createClinicalTrialDto);
        if (!validationResult.IsValid)
            throw new ValidationException(string.Join("\n", validationResult.Errors));

        return createClinicalTrialDto;
    }
}