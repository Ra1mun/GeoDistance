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

    public GeoDistanceController(IDistanceService distanceService)
    {
        _distanceService = distanceService;
    }

    [HttpGet]
    public async Task<DistanceModel> GetDistance([FromQuery] [Required] IataModel model)
    {
        return await _distanceService.GetDistance(model);
    }
}