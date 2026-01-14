namespace LegacyOrderService.Models
{
    public class Order
    {
        public string CustomerName { get; }
        public string ProductName { get; }
        public int Quantity { get; }
        public double Price { get; }
        public double Total => Quantity * Price;

        private Order(string customerName, string productName, int quantity, double price)
        {
            CustomerName = customerName;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public static Order Create(string customerName, string productName, int quantity, double price)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name cannot be empty.", nameof(customerName));

            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.", nameof(productName));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.", nameof(quantity));

            if (price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(price));

            return new Order(customerName, productName, quantity, price);
        }
    }
}
