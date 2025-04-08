using Microsoft.AspNetCore.Mvc;
using Moq;
using CalculationService.Controllers;
using CalculationService.Models;
using Microsoft.Extensions.Logging;
using Calculationervice.Services;
using CalculationService.Services;
using CalculationService.Model;

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
                new ElectricityTariff () { Provider = _providerName, Name = "Product A", Type = 1, BaseCost = 10, AdditionalKwhCost = 25 },
                new ElectricityTariff () { Provider = _providerName, Name = "Product B", Type = 2, IncludedKwh = 4000, BaseCost = 850, AdditionalKwhCost = 35 }
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
    }
}