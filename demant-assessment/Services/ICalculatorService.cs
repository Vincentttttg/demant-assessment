using demant_assessment.Models;

namespace demant_assessment.Services
{
    public interface ICalculatorService
    {
        decimal CalculateFinalAmount(Order order);
    }
}
