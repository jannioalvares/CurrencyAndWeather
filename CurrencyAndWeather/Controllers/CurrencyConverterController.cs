using CurrencyAndWeather.Model;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAndWeather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly Dictionary<string, string> cityMapping = new Dictionary<string, string>
        {
            { "IDR", "jakarta" },
            { "USD", "newyork" },
            { "JPY", "tokyo" },
            { "EUR", "paris" },
            { "SGD", "singapore" },
            { "MYR", "kualalumpur" },
            { "CAD", "ottawa" },
            { "AUD", "canberra" },
            { "GBP", "london" },
            { "CNY", "beijing" },
            { "INR", "newdelhi" },
            { "NZD", "wellington" },
            { "CHF", "bern" },
            { "KRW", "seoul" },
            { "BRL", "brasília" },
            { "RUB", "moscow" },
            { "ZAR", "pretoria" },
            { "SEK", "stockholm" },
            { "AED", "abudhabi" },
            { "ARS", "buenosaires" },
            { "MXN", "mexicocity" }
        };
        
        [HttpGet("{fromCurrency}/{toCurrency}/{amount}")]
        public async Task<IActionResult> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            using (var httpClient = new HttpClient())
            {
                var api = "https://v6.exchangerate-api.com/v6/ad7a23b99163f9122b4f887a/latest/" + fromCurrency;
                var response = await httpClient.GetAsync(api);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeRateApiResponse>(json);

                    if (apiResponse.Conversion_rates.ContainsKey(toCurrency))
                    {
                        var conversionRate = apiResponse.Conversion_rates[toCurrency];
                        var convertedAmount = amount * conversionRate;
                        var formattedAmount = Math.Round(convertedAmount, 2);

                        var city = cityMapping.TryGetValue(toCurrency, out var cityName) ? cityName : null;

                        if (!string.IsNullOrEmpty(city))
                        {
                            var weatherApiKey = "SKID2RkjLODvb4dS9djzLxmupQkj6ZGW";
                            var citySearchApi = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={weatherApiKey}&q={city}";
                            var weatherResponse = await httpClient.GetAsync(citySearchApi);

                            if (weatherResponse.IsSuccessStatusCode)
                            {
                                var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
                                var weatherApiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeatherApiResponse>>(weatherJson);


                                var cityKey = weatherApiResponse[0].Key;
                                var weatherApi = $"http://dataservice.accuweather.com/currentconditions/v1/{cityKey}?apikey={weatherApiKey}";
                                var weatherDataResponse = await httpClient.GetAsync(weatherApi);

                                if (weatherDataResponse.IsSuccessStatusCode)
                                {
                                    var weatherDataJson = await weatherDataResponse.Content.ReadAsStringAsync();
                                    var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WeatherData>>(weatherDataJson);


                                    var weatherText = weatherData[0].WeatherText;
                                    var temperature = weatherData[0].Temperature.Metric.Value;

                                    return Ok(new
                                    {
                                        From = fromCurrency,
                                        To = toCurrency,
                                        Amount = amount,
                                        Conversion = formattedAmount,
                                        Weather = weatherText,
                                        Temperature = temperature
                                    });

                                }

                            }
                        }

                        return NotFound($"City for currency code '{toCurrency}' not found");
                    }

                    return NotFound($"Currency code '{toCurrency}' not found");
                }

                return BadRequest(response.StatusCode);
            }
        }
    }
}
