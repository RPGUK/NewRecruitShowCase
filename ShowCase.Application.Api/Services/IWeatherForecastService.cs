using ShowCase.Application.Api.Data;
using ShowCase.Application.Library;

namespace ShowCase.Application.Api.Services;

public interface IWeatherForecastService
{
    Task<WeatherForecast[]> GetForecastAsync();
    Task GenerateRandomDataAsync();
    
}