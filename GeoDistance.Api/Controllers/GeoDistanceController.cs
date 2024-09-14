namespace GeoDistance.Api.Controllers;

using GeopositionDistace;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GeoDistanceController : ControllerBase
{
    private readonly IDistanceService _distanceService;

    public GeoDistanceController(IDistanceService distanceService)
    {
        _distanceService = distanceService;
    }

    [HttpGet]
    public async Task<double> GetDistance(string firstIATA, string secondIATA)
    {
        return await _distanceService.GetDistance(firstIATA, secondIATA);
    }
}