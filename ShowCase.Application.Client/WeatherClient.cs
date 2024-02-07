using ShowCase.Application.Client.Data;
using System.Text.Json;

namespace ShowCase.Application.Client
{
    public partial class WeatherClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;

        public WeatherClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecastAsync()
        {
            var response = await _httpClient.GetAsync("WeatherForecast");
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(responseStream);
        }
    }
}
