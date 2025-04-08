using CalculationService.Models;
using CalculationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalculationService.Controllers
{
    [ApiController]
    [Route("api/calculate")]
    public class CalculationController : ControllerBase
    {
        private readonly IElectricityProviderService _electricityProviderService;
        private readonly ILogger<CalculationController> _logger;

        public CalculationController(IElectricityProviderService electricityProviderService, ILogger<CalculationController> logger)
        {
            _electricityProviderService = electricityProviderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCalculation([FromQuery] int kwh)
        {
            if (kwh <= 0)
            {
                return BadRequest("Please enter a valid kWh value (greater than zero).");
            }

            var tariffs = await _electricityProviderService.GetElectricityTariffsAsync();
            if (tariffs == null || tariffs.Count == 0)
            {
                return NotFound("No electricity tariffs found.");
            }

            var results = tariffs.Select(tariff => new ConsumptionTariff(
                    tariff.Provider,
                    tariff.Name,
                    tariff.Type,
                    CalculateAnnualCost(tariff, kwh)
                )).ToList();

            return Ok(results);
        }

        private decimal CalculateAnnualCost(ElectricityTariff tariff, int kwh)
        {
            if (tariff.Type == 1) // Basic
            {
                return (decimal)(tariff.BaseCost * 12 + (tariff.AdditionalKwhCost / 100 * kwh));
            }
            else if (tariff.Type == 2) // Package
            {
                if (kwh <= tariff.IncludedKwh)
                    return (decimal)tariff.BaseCost;
                else
                    return (decimal)(kwh <= tariff.IncludedKwh ? tariff.BaseCost : tariff.BaseCost + (tariff.AdditionalKwhCost / 100 * (kwh - tariff.IncludedKwh)));
            }
            return 0;
        }
    }
}