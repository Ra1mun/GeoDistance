using GeoDistance;
using Microsoft.AspNetCore.Mvc;

namespace GeopositionDistance.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class GeoDistanceController : ControllerBase
{
    private IDistanceService _distanceService;

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