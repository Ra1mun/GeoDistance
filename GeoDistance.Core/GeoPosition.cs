namespace GeopositionDistace;

public record GeoPosition
{
    private readonly string _iata;

    public string IATA
    {
        get => _iata;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException(nameof(value));

            _iata = value;
        }
    }

    public Location Location { get; init; }
}