using System.Transactions;

namespace ElectricityServiceApi.Models
{
    public class ElectricityTariff
    {
        // new { Provider = providerName, Name = "Product A", Type = 1, BaseCost = 5, AdditionalKwhCost = 22 },
        public string Provider { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public double BaseCost { get; set; }
        public double AdditionalKwhCost { get; set; }
        public int IncludedKwh { get; set; }

        public ElectricityTariff()
        {
            
        }

        public ElectricityTariff(string provider, string name, int type, double baseCost, double additionalKwhCost, int includedKwh = 0)
        {
            Provider = provider;
            Name = name;
            Type = type;
            BaseCost = baseCost;
            AdditionalKwhCost = additionalKwhCost;
            IncludedKwh = includedKwh;
            
        }

    }
}