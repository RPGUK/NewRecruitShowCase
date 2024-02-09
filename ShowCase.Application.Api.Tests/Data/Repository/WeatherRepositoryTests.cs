using Microsoft.Extensions.Logging;
using Moq;

namespace ShowCase.Application.Api.Data.Repository.Tests
{
    [TestClass]
    public class WeatherRepositoryTests
    {

        private WeatherRepository _repository;
        private Mock<ILogger<WeatherRepository>> _mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            _mockLogger = new Mock<ILogger<WeatherRepository>>();
            _repository = new WeatherRepository(_mockLogger.Object);

            _repository.Add(new WeatherForecast { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 20, Summary = "Warm" });
            _repository.Add(new WeatherForecast { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 10, Summary = "Cool" });
            _repository.Add(new WeatherForecast { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 30, Summary = "Hot" });

        }

        [TestMethod]
        public void Add_WeatherForecast_AddsSuccessfully()
        {
            var newForecast = new WeatherForecast {Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 40, Summary = "Scorching"};
            _repository.Add(newForecast);

            var result = _repository.GetById(newForecast.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(newForecast.Id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Add_ExistingWeatherForecast_ThrowsException()
        {
            var existingForecast = _repository.GetAll().First();
            _repository.Add(existingForecast);
        }

        [TestMethod]
        public void Delete_WeatherForecast_DeletesSuccessfully()
        {
            var forecastToDelete = _repository.GetAll().First();
            _repository.Delete(forecastToDelete);

            var result = _repository.GetById(forecastToDelete.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAll_ReturnsAllForecasts()
        {
            var results = _repository.GetAll();
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void GetById_ExistingId_ReturnsForecast()
        {
            var existingForecast = _repository.GetAll().First();
            var result = _repository.GetById(existingForecast.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(existingForecast.Id, result.Id);
        }

        [TestMethod]
        public void GetById_NonExistingId_ReturnsNull()
        {
            var result = _repository.GetById(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Update_ExistingWeatherForecast_UpdatesSuccessfully()
        {
            // Step 1: Retrieve an existing WeatherForecast
            var existingForecast = _repository.GetAll().First();
            var originalTemperature = existingForecast.TemperatureC;
            var originalSummary = existingForecast.Summary;

            // Step 2: Modify some properties
            var newTemperature = originalTemperature + 5; // Increase temperature by 5
            var newSummary = "Updated " + originalSummary;
            existingForecast.TemperatureC = newTemperature;
            existingForecast.Summary = newSummary;

            // Step 3: Update the forecast in the repository
            _repository.Update(existingForecast);

            // Step 4: Retrieve the updated forecast and assert changes
            var updatedForecast = _repository.GetById(existingForecast.Id);
            Assert.IsNotNull(updatedForecast);
            Assert.AreEqual(newTemperature, updatedForecast.TemperatureC);
            Assert.AreEqual(newSummary, updatedForecast.Summary);
        }

        [TestMethod]
        public void Update_NonExistingWeatherForecast_NoChange()
        {
            var nonExistingForecast = new WeatherForecast { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 40, Summary = "Scorching" };
            _repository.Update(nonExistingForecast);

            var result = _repository.GetById(nonExistingForecast.Id);
            Assert.IsNull(result);
        }
    }
}