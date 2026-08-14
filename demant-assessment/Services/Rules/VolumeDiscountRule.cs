using demant_assessment.Models;

namespace demant_assessment.Services.Rules
{
    public class VolumeDiscountRule : IPricingRule
    {
        private const int VolumeDiscountQuantity = 10;
        private const decimal VolumeDiscountPercentage = 0.1m;

        public decimal Apply(Order order, decimal currentTotal)
        {
            foreach (var orderLine in order.OrderLines)
            {
                currentTotal += orderLine.Quantity >= VolumeDiscountQuantity
                    ? orderLine.TotalBaseUnitPrice * (1 - VolumeDiscountPercentage)
                    : orderLine.TotalBaseUnitPrice;
            }

            return currentTotal;
        }
    }
}
