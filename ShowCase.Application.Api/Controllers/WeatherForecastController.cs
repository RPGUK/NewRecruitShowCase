using Microsoft.AspNetCore.Mvc;
using ShowCase.Application.Api.Data;
using ShowCase.Application.Api.Services;

namespace ShowCase.Application.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var weatherArray = await _weatherForecastService.GetForecastAsync();
            var weatherList = weatherArray.ToList();

            return weatherList;
        }

        [HttpPost("generateRandomWeatherData", Name = "GenerateRandomData")]
        public async Task<IActionResult> GenerateData()
        {
            await _weatherForecastService.GenerateRandomDataAsync();
            return Ok();
        }

    }
}
