using demant_assessment.Models;
using demant_assessment.Services;
using Microsoft.AspNetCore.Mvc;

namespace demant_assessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;
        public OrderController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpPost("price")]
        public async Task<IActionResult> Price([FromBody] Order order)
        {
            try
            {
                return Ok(new
                {
                    totalAmount = _calculatorService.CalculateFinalAmount(order)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }
    }
}
