namespace LegacyOrderService.Models
{
    public sealed record OrderReadModel(
        string CustomerName,
        string ProductName,
        int Quantity,
        decimal Price,
        decimal Total
    )
    {
    }
}
