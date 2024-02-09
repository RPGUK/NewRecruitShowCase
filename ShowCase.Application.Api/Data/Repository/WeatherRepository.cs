
using System.Collections.Concurrent;
using System.Text.Json;

namespace ShowCase.Application.Api.Data.Repository
{
    public class WeatherRepository : IRepository<WeatherForecast>
    {
        public static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ConcurrentDictionary<Guid, WeatherForecast> _weatherForecasts = new();
        private readonly ILogger<WeatherRepository> _logger;
        private readonly ReaderWriterLockSlim _lock = new();

        public WeatherRepository(ILogger<WeatherRepository> logger)
        {
            _logger = logger;
            ReadJson();
        }

        public void Add(WeatherForecast entity)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_weatherForecasts.ContainsKey(entity.Id))
                {
                    throw new Exception("Id already exists");
                }
                _weatherForecasts[entity.Id] = entity; // Add operation is thread-safe
                WriteJson();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Delete(WeatherForecast entity)
        {
            _lock.EnterWriteLock();
            try
            {
                _weatherForecasts.TryRemove(entity.Id, out _); // Remove operation is thread-safe
                WriteJson();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public IEnumerable<WeatherForecast> GetAll()
        {
            _lock.EnterReadLock();
            try
            {
                return _weatherForecasts.Values.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public WeatherForecast GetById(Guid id)
        {
            _lock.EnterReadLock();
            try
            {
                _weatherForecasts.TryGetValue(id, out WeatherForecast forecast);
                return forecast;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Update(WeatherForecast entity)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_weatherForecasts.ContainsKey(entity.Id))
                {
                    _weatherForecasts[entity.Id] = entity; // Update operation is thread-safe
                    WriteJson();
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private void ReadJson()
        {
            var data = File.ReadAllText("Data/weather.json");

            try
            {
                var forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(data);
                if (forecasts != null)
                {
                    _weatherForecasts = new ConcurrentDictionary<Guid, WeatherForecast>(
                        forecasts.ToDictionary(forecast => forecast.Id));
                }
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
            var data = JsonSerializer.Serialize(_weatherForecasts.Values, options);
            File.WriteAllText("Data/weather.json", data);
        }
    }


    //public class WeatherRepository : IRepository<WeatherForecast>
    //{
    //    public static readonly string[] Summaries = new[]
    //    {
    //        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //    };

    //    private List<WeatherForecast> _weatherForecasts = new();
    //    private readonly ILogger<WeatherRepository> _logger;

    //    public WeatherRepository(ILogger<WeatherRepository> logger)
    //    {
    //        _logger = logger;
    //        ReadJson();
    //    }

    //    public void Add(WeatherForecast entity)
    //    {
    //        if (_weatherForecasts.Any(x => x.Id == entity.Id))
    //        {
    //            throw new Exception("Id already exists");
    //        }

    //        _weatherForecasts.Add(entity);
    //        WriteJson();
    //    }

    //    public void Delete(WeatherForecast entity)
    //    {
    //        _weatherForecasts.Remove(entity);
    //        WriteJson();
    //    }

    //    public IEnumerable<WeatherForecast> GetAll()
    //    {
    //        return _weatherForecasts;
    //    }

    //    public WeatherForecast GetById(Guid id)
    //    {
    //        return _weatherForecasts.FirstOrDefault(x => x.Id == id);
    //    }

    //    public void Update(WeatherForecast entity)
    //    {
    //        var index = _weatherForecasts.FindIndex(x => x.Id == entity.Id);
    //        if (index >= 0)
    //        {
    //            _weatherForecasts[index] = entity;
    //            WriteJson();
    //        }
    //    }

    //    private void ReadJson()
    //    {
    //        var data = File.ReadAllText("Data/weather.json");

    //        try
    //        {
    //            _weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(data);
    //        }
    //        catch (Exception e)
    //        {
    //            _logger.LogError(e, "Error loading weather data");
    //        }
    //    }

    //    private void WriteJson()
    //    {
    //        var options = new JsonSerializerOptions
    //        {
    //            WriteIndented = true
    //        };
    //        var data = JsonSerializer.Serialize(_weatherForecasts, options);
    //        File.WriteAllText("Data/weather.json", data);
    //    }
    //}
}
