namespace LegacyOrderService.Models
{
    public sealed record OrderReadModel(
        string CustomerName,
        string ProductName,
        int Quantity,
        decimal Price
    )
    {
        public decimal Total => Quantity * Price;
    }
}
