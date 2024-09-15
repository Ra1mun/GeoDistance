namespace GeoDistance.Core.Dto;

public record IataModel
{
    public string FirstAirport { get; init; }

    public string SecondAirport { get; init; }
}