using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TestAssignment.API.Filters;
using TestAssignment.Application.Services;
using TestAssignment.Domain.Entities;

namespace TestAssignment.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClinicalTrialsController : Controller
{
    private readonly IClinicalTrialService _clinicalTrialService;
    private readonly ILogger<ClinicalTrialsController> _logger;

    public ClinicalTrialsController(ILogger<ClinicalTrialsController> logger,
        IClinicalTrialService clinicalTrialService)
    {
        _logger = logger;
        _clinicalTrialService = clinicalTrialService;
    }

    [HttpPost]
    [FileValidationFilterAttribute([".json"], 2 * 1024 * 1024)]
    public async Task<IActionResult> UploadFromFileAsync(IFormFile file,
        CancellationToken cancellationToken = default)
    {
        using var stream = new StreamReader(file.OpenReadStream());
        var jsonData = await stream.ReadToEndAsync();

        await _clinicalTrialService.CreateFromFileAsync(jsonData, cancellationToken);

        return Accepted();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync([Range(1, long.MaxValue)] long id,
        CancellationToken cancellationToken = default)
    {
        var clinicalTrial = await _clinicalTrialService.GetAsync(id, cancellationToken);

        if (clinicalTrial == null)
            return NotFound();

        return Ok(clinicalTrial);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] [EnumDataType(typeof(Status))] Status? status,
        [FromQuery] [Range(1, int.MaxValue)] int pageNumber = 1, [FromQuery] [Range(1, 1000)] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var clinicalTrials = await _clinicalTrialService.GetAsync(status, pageNumber, pageSize, cancellationToken);
        return Ok(clinicalTrials);
    }
}