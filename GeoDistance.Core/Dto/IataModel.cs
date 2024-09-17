namespace GeoDistance.Core.Dto;

public record IataModel
{
    public string FirstAirport { get; init; } = string.Empty;

    public string SecondAirport { get; init; } = string.Empty;
}