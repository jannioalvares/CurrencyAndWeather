namespace CurrencyAndWeather.Model
{
    public class ExchangeRateApiResponse
    {
        public string Result { get; set; }
        public Dictionary<string, decimal> Conversion_rates { get; set; }
    }
}
