using System.Transactions;

namespace CalculationService.Models
{
    public class ElectricityTariff
    {
        public string Provider { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public double BaseCost { get; set; }
        public double AdditionalKwhCost { get; set; }
        public int IncludedKwh { get; set; }

    }
}
