using System.ComponentModel.DataAnnotations;

namespace demant_assessment.Models
{
    public class OrderLine
    {
        public string ProductCode { get; set; }

        public string ProductType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "UnitPrice must be non-negative.")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalBaseUnitPrice => UnitPrice * Quantity;

        public OrderLine()
        {

        }

        public OrderLine(string productCode, string productType, decimal unitPrice, int quantity)
        {
            ProductCode = productCode;
            ProductType = productType;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
