namespace GeopositionDistace
{
    public record GeoPosition
    {
        public string IATA
        {
            get => _iata;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException(nameof(value));
                }

                _iata = value;
            }
        }
        
        public Location Location { get; init; }

        private string _iata;
    }
}
