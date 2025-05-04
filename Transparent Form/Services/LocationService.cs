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
    public class LocationService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string NominatimUrl = "https://nominatim.openstreetmap.org/search";

        public async Task<(double Lat, double Long)> GetCoordinatesAsync(string address)
        {
            try
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
                {
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourUniversityApp/1.0");
                }

                string url = $"{NominatimUrl}?q={Uri.EscapeDataString(address)}&format=json";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonContent = await response.Content.ReadAsStringAsync();
                List<NominatimResponse> locations = JsonSerializer.Deserialize<List<NominatimResponse>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (locations?.Count > 0)
                {
                    return (double.Parse(locations[0].Lat),
                           double.Parse(locations[0].Lon));
                }
                return (0, 0);
            }
            catch
            {
                return (0, 0);
            }
        }
    }

    public class NominatimResponse
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("lon")]
        public string Lon { get; set; }
    }
}