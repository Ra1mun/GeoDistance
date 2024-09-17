namespace GeoDistance.Api.Controllers;

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using GeoDistance.Core.Dto;
using GeoDistance.Core.Services;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/v1/")]
[Produces(MediaTypeNames.Application.Json)]
public class GeoDistanceController : ControllerBase
{
    private readonly IDistanceService _distanceService;
    private readonly IIataModelValidationService _validationService;

    public GeoDistanceController(IDistanceService distanceService, IIataModelValidationService validationService)
    {
        _distanceService = distanceService;
        _validationService = validationService;
    }

    [HttpGet]
    public async Task<ActionResult<DistanceModel>> GetDistance(
        [FromQuery] [Required] IataModel model,
        CancellationToken cancellationToken = default)
    {
        if (!_validationService.Validate(model))
            return BadRequest("Validation error");

        return await _distanceService.GetDistanceAsync(model, cancellationToken);
    }
}