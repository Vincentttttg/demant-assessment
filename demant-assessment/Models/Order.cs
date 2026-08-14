namespace demant_assessment.Models
{
    public class Order
    {
        public List<OrderLine> OrderLines { get; set; } = [];

        public Order()
        {

        }

        public Order(params OrderLine[] orderLines)
        {
            OrderLines = orderLines.ToList();
        }
        
    }
}
