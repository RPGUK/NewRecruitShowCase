using ShowCase.Application.Api.Data;
using ShowCase.Application.Api.Data.Repository;
using ShowCase.Application.Library;

namespace ShowCase.Application.Api.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {

        private readonly IRepository<WeatherForecast> _weatherRepository;

        public WeatherForecastService(IRepository<WeatherForecast> weatherRepository)
        {
                _weatherRepository = weatherRepository;
        }

        public Task<WeatherForecast[]> GetForecastAsync()
        {
            return Task.FromResult(_weatherRepository.GetAll().ToArray());
        }

        public Task GenerateRandomDataAsync()
        {
            var rng = new Random();
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = WeatherRepository.Summaries[rng.Next(WeatherRepository.Summaries.Length)]
            }).ToArray();

            foreach (var item in data)
            {
                _weatherRepository.Add(item);
            }

            return Task.CompletedTask;

        }

    }
}
