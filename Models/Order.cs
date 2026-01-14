using LegacyOrderService.Exceptions;

namespace LegacyOrderService.Models
{
    public class Order
    {
        public string CustomerName { get; }
        public string ProductName { get; }
        public int Quantity { get; }
        public decimal Price { get; }

        private Order(string customerName, string productName, int quantity, decimal price)
        {
            CustomerName = customerName;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public static Order Create(string customerName, string productName, int quantity, decimal price)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new DomainValidationException("Customer name cannot be empty.");

            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainValidationException("Product name cannot be empty.");

            if (quantity <= 0)
                throw new DomainValidationException("Quantity must be positive.");

            if (price < 0)
                throw new DomainValidationException("Price cannot be negative.");

            return new Order(customerName, productName, quantity, price);
        }
    }
}
