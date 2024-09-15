namespace GeoDistance.Core.Dto;

using System.Text.Json.Serialization;

public record struct Location
{
    [JsonPropertyName("Lat")]
    public double Latitude { get; init; }

    [JsonPropertyName("Lon")]
    public double Longitude { get; init; }
}