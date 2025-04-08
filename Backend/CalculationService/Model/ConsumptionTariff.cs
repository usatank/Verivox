namespace CalculationService.Model
{
    public class ConsumptionTariff
    {
        public string Provider { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal AnnualCost { get; set; }

        public ConsumptionTariff(string provider, string name, int type, decimal annualCost)
        {
            Provider = provider;
            Name = name;
            Type = type;
            AnnualCost = annualCost;
        }
    }
}
