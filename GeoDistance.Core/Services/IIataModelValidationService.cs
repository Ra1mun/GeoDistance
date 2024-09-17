namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public interface IIataModelValidationService
{
    bool Validate(IataModel model);
}