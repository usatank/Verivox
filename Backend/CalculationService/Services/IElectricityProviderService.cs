using CalculationService.Models;

namespace CalculationService.Services
{
    public interface IElectricityProviderService
    {
        public Task<List<ElectricityTariff>> GetElectricityTariffsAsync();
    }
}
