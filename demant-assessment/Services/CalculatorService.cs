using demant_assessment.Models;
using demant_assessment.Services.Rules;

namespace demant_assessment.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IEnumerable<IPricingRule> _pricingRules;

        public CalculatorService(IEnumerable<IPricingRule> pricingRules)
        {
            _pricingRules = pricingRules;
        }

        public decimal CalculateFinalAmount(Order order)
        {
            if (order == null)
            {
                return 0;
            }

            decimal totalAmount = 0;

            foreach (var rule in _pricingRules)
            {
                totalAmount = rule.Apply(order, totalAmount);
            }

            return Math.Round(totalAmount, 2);
        }
    }
}
