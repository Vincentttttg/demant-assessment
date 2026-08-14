using demant_assessment.Models;

namespace demant_assessment.Services.Rules
{
    public class HighValueDiscountRule : IPricingRule
    {
        private const int HighValueDiscountThreshold = 10000;
        private const decimal HighValueDiscountPercentage = 0.05m;

        public decimal Apply(Order order, decimal currentTotal)
        {
            return currentTotal >= HighValueDiscountThreshold
                ? currentTotal * (1 - HighValueDiscountPercentage)
                : currentTotal;
        }
    }
}
