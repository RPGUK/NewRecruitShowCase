
using System.Text.Json;

namespace ShowCase.Application.Api.Data.Repository
{
    public class WeatherRepository : IRepository<WeatherForecast>
    {
        public static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private List<WeatherForecast> _weatherForecasts = new();
        private readonly ILogger<WeatherRepository> _logger;

        public WeatherRepository(ILogger<WeatherRepository> logger)
        {
            _logger = logger;
            ReadJson();
        }

        public void Add(WeatherForecast entity)
        {
            if (_weatherForecasts.Any(x => x.Id == entity.Id))
            {
                throw new Exception("Id already exists");
            }

            _weatherForecasts.Add(entity);
            WriteJson();
        }

        public void Delete(WeatherForecast entity)
        {
            _weatherForecasts.Remove(entity);
            WriteJson();
        }

        public IEnumerable<WeatherForecast> GetAll()
        {
            return _weatherForecasts;
        }

        public WeatherForecast GetById(Guid id)
        {
            return _weatherForecasts.FirstOrDefault(x => x.Id == id);
        }

        public void Update(WeatherForecast entity)
        {
            var index = _weatherForecasts.FindIndex(x => x.Id == entity.Id);
            if (index >= 0)
            {
                _weatherForecasts[index] = entity;
                WriteJson();
            }
        }

        private void ReadJson()
        {
            var data = File.ReadAllText("Data/weather.json");

            try
            {
                _weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading weather data");
            }
        }

        private void WriteJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var data = JsonSerializer.Serialize(_weatherForecasts, options);
            File.WriteAllText("Data/weather.json", data);
        }
    }
}
