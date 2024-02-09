using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShowCase.Application.Api.Data.Repository;
using ShowCase.Application.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowCase.Application.Api.Data;

namespace ShowCase.Application.Api.Services.Tests
{
    [TestClass()]
    public class WeatherForecastServiceTests
    {
        private Mock<IRepository<WeatherForecast>> _mockRepository;
        private WeatherForecastService _service;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<WeatherForecast>>();
            _service = new WeatherForecastService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetForecastAsync_ReturnsCorrectData()
        {
            // Arrange
            var mockData = new[] {
                new WeatherForecast { Id = Guid.NewGuid(), Date = DateTime.Now, TemperatureC = 25, Summary = "Warm" },
                
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(mockData);

            // Act
            var forecasts = await _service.GetForecastAsync();

            // Assert
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
            Assert.AreEqual(mockData.Length, forecasts.Length);
            
        }

        [TestMethod]
        public async Task GenerateRandomDataAsync_AddsDataToRepository()
        {
            // Act
            await _service.GenerateRandomDataAsync();

            // Assert
            _mockRepository.Verify(repo => repo.Add(It.IsAny<WeatherForecast>()), Times.Exactly(5));
        }
    }
}