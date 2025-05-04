using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Transparent_Form.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

        public async Task<string> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            try
            {
                string url = $"{BaseUrl}?latitude={latitude}&longitude={longitude}" +
                            "&current_weather=true&timezone=auto";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonContent = await response.Content.ReadAsStringAsync();
                WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return FormatWeather(weatherResponse.CurrentWeather);
            }
            catch
            {
                return "Weather data unavailable";
            }
        }

        private string FormatWeather(CurrentWeather weather)
        {
            return $"{weather.Temperature}°C, {GetWeatherDescription(weather.WeatherCode)}";
        }

        private string GetWeatherDescription(int code)
        {
            string description;

            if (code == 0)
                description = "Clear sky";
            else if (code == 1)
                description = "Partly cloudy";
            else if (code == 2)
                description = "Cloudy";
            else if (code == 3)
                description = "Overcast";
            else if (code == 45)
                description = "Fog";
            else if (code == 61)
                description = "Light rain";
            else if (code == 80)
                description = "Rain showers";
            else
                description = "Unknown weather";

            return description;
        }
    }

    public class WeatherResponse
    {
        [JsonPropertyName("current_weather")]
        public CurrentWeather CurrentWeather { get; set; }
    }

    public class CurrentWeather
    {
        public float Temperature { get; set; }
        [JsonPropertyName("weathercode")]
        public int WeatherCode { get; set; }
    }
}