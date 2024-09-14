using System.Text.Json.Serialization;

namespace GeopositionDistace;

public record struct Location
{
    [JsonPropertyName("Lat")]
    public double Latitude { get; init; }
    
    [JsonPropertyName("Lon")]
    public double Longitude { get; init; }
}