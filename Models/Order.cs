namespace LegacyOrderService.Models
{
    public class Order
    {
        public required string CustomerName { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total => Quantity * Price;
    }
}
