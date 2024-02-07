using ShowCase.Application.Client.Data;
using ShowCase.Application.Library;

namespace ShowCase.Application.Client;

public interface IWeatherClient
{
    Task<IEnumerable<WeatherForecast>?> GetWeatherForecastAsync();
}