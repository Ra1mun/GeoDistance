namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public class IataModelValidationService : IIataModelValidationService
{
    public bool Validate(IataModel model)
    {
        return !string.IsNullOrEmpty(model.FirstAirport) && !string.IsNullOrEmpty(model.SecondAirport);
    }
}