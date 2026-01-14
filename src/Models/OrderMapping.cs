namespace LegacyOrderService.Models
{
    internal static class OrderMapping
    {
        public static OrderReadModel ToReadModel(this Order order)
        {
            return new OrderReadModel(
                order.CustomerName,
                order.ProductName,
                order.Quantity,
                order.Price
            );
        }
    }
}
