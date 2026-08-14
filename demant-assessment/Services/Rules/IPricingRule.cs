using demant_assessment.Models;

namespace demant_assessment.Services.Rules
{
    public interface IPricingRule
    {
        decimal Apply(Order order, decimal currentTotal);
    }
}
