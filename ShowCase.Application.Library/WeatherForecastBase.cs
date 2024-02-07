using System.Text.Json.Serialization;

namespace ShowCase.Application.Library
{
    public abstract class WeatherForecastBase
    {

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }
    }
}
