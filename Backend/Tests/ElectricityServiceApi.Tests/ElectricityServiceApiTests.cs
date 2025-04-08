using Xunit;
using Moq;
using ElectricityServiceApi.Controllers;
using ElectricityServiceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ElectricityServiceApi.Tests
{
    public class ElectricityServiceApiTests
    {
        private readonly string _providerName = "";
        private readonly Mock<ILogger<ElecticityServiceController>> _logger;
        private readonly List<ElectricityTariff> _electricityTariffs;

        public ElectricityServiceApiTests()
        {
            _logger = new Mock<ILogger<ElecticityServiceController>>();
            _providerName = "Test Provider";
            _electricityTariffs = new List<ElectricityTariff>
            {
                new ElectricityTariff () { Provider = _providerName, Name = "Product A", Type = 1, BaseCost = 10, AdditionalKwhCost = 25 },
                new ElectricityTariff () { Provider = _providerName, Name = "Product B", Type = 2, IncludedKwh = 4000, BaseCost = 850, AdditionalKwhCost = 35 }
            };

        }

        [Fact]
        public void GetTariffs_ReturnsListOfTariffs()
        {
           
            
            // Arrange
            var controller = new ElecticityServiceController(_providerName, _electricityTariffs, _logger.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<JsonHttpResult<List<ElectricityTariff>>>(result);

            // Extract JSON value and verify contents
            var jsonResult = result as JsonHttpResult<List<ElectricityTariff>>;
            
            Assert.NotNull(jsonResult);
            Assert.Equal(_electricityTariffs.Count, jsonResult.Value.Count);
            Assert.Equal(_electricityTariffs[0].Provider, jsonResult.Value[0].Provider);
            Assert.Equal(_electricityTariffs[0].Name, jsonResult.Value[0].Name);
            Assert.Equal(_electricityTariffs[0].Type, jsonResult.Value[0].Type);
            Assert.Equal(_electricityTariffs[0].BaseCost, jsonResult.Value[0].BaseCost);
            Assert.Equal(_electricityTariffs[0].AdditionalKwhCost, jsonResult.Value[0].AdditionalKwhCost);
        }
    }
}