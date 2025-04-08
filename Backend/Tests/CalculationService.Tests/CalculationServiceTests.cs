using Microsoft.AspNetCore.Mvc;
using Moq;
using CalculationService.Controllers;
using CalculationService.Models;
using Microsoft.Extensions.Logging;
using CalculationService.Services;

namespace CalculationService.Tests
{
    public class CalculationServiceTests
    {
        private readonly string _providerName = "";
        private readonly Mock<ILogger<CalculationController>> _logger;
        private readonly List<ElectricityTariff> _electricityTariffs;
        private readonly Mock<IElectricityProviderService> _mockService;

        public CalculationServiceTests()
        {
            _logger = new Mock<ILogger<CalculationController>>();
            _providerName = "Test Provider";
            _electricityTariffs = new List<ElectricityTariff>
            {
                new ElectricityTariff () { Provider = _providerName, Name = "Product A", Type = 1, BaseCost = 5, AdditionalKwhCost = 22 },
                new ElectricityTariff () { Provider = _providerName, Name = "Product B", Type = 2, IncludedKwh = 4000, BaseCost = 800, AdditionalKwhCost = 30 }
            };
            _mockService  = new Mock<IElectricityProviderService>();
            _mockService.Setup(s => s.GetElectricityTariffsAsync())
                .ReturnsAsync(_electricityTariffs);
        }

        [Fact]
        public async Task CalculateCost_ReturnsExpectedResults()
        {
            // Arrange
            var controller = new CalculationController(_mockService.Object, _logger.Object);

            // Act
            var result = await controller.GetCalculation(1000) as OkObjectResult;
            var costs = result?.Value as List<ConsumptionTariff>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(costs);
            Assert.Equal(2, costs.Count);
            Assert.Equal(_electricityTariffs[0].Provider, costs[0].Provider);
            Assert.Equal(_electricityTariffs[0].Name, costs[0].Name);
            Assert.Equal(_electricityTariffs[0].Type, costs[0].Type);
        }

        [Fact]
        public async Task CalculateCost_BadRequestInCaseOfZeroInput()
        {
            // Arrange
            var controller = new CalculationController(_mockService.Object, _logger.Object);

            // Act
            var result = await controller.GetCalculation(0) as BadRequestObjectResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Please enter a valid kWh value (greater than zero).", result.Value?.ToString());
        }

        [Fact]
        public async Task CalculateCost_BadRequestInCaseOfNegativeInput()
        {
            // Arrange
            var controller = new CalculationController(_mockService.Object, _logger.Object);

            // Act
            var result = await controller.GetCalculation(-1) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Please enter a valid kWh value (greater than zero).", result.Value?.ToString());
        }

        [Fact]
        public async Task CalculateCost_NotFoundInCaseOfNullOrEmptyTariffs()
        {
            // Arrange
            _mockService.Setup(s => s.GetElectricityTariffsAsync())
                .ReturnsAsync((List<ElectricityTariff>?)null);
            var controller = new CalculationController(_mockService.Object, _logger.Object);

            // Act
            var result = await controller.GetCalculation(2500) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            
        }

        [Fact]
        public async Task CalculateCost_NotFoundInCaseOfEmptyTariffs()
        {
            // Arrange
            _mockService.Setup(s => s.GetElectricityTariffsAsync())
                .ReturnsAsync(new List<ElectricityTariff>());
            var controller = new CalculationController(_mockService.Object, _logger.Object);

            // Act
            var result = await controller.GetCalculation(2500) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            
        }
    }
}