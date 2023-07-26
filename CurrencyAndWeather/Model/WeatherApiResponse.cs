namespace CurrencyAndWeather.Model
{
    public class WeatherApiResponse
    {
        public string Key { get; set; }
    }

    public class WeatherData
    {
        public TemperatureData Temperature { get; set; }
        public string WeatherText { get; set; }
    }

    public class TemperatureData
    {
        public MetricData Metric { get; set; }
    }

    public class MetricData
    {
        public decimal Value { get; set; }
    }

}
