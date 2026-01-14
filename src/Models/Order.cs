using LegacyOrderService.Exceptions;

namespace LegacyOrderService.Models
{
    public class Order
    {
        public long Id { get; private set; }
        public string CustomerName { get; }
        public string ProductName { get; }
        public int Quantity { get; }
        public decimal Price { get; }
        public decimal Total => Quantity * Price;

        private Order(string customerName, string productName, int quantity, decimal price)
        {
            CustomerName = customerName;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        /// <summary>
        /// Creates a validated <see cref="Order"/> instance.
        /// </summary>
        /// <param name="customerName">Customer name. Must not be null, empty or whitespace.</param>
        /// <param name="productName">Product name. Must not be null, empty or whitespace.</param>
        /// <param name="quantity">Quantity ordered. Must be greater than zero.</param>
        /// <param name="price">Unit price. Must be zero or a positive value.</param>
        /// <returns>A new <see cref="Order"/> populated with the provided values.</returns>
        /// <exception cref="DomainValidationException">Thrown when any argument fails validation (empty names, non-positive quantity, or negative price).</exception>
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

        public void AssignId(long id)
        {
            if (Id != 0)
                throw new DomainValidationException("Order ID already assigned.");

            Id = id;
        }
    }
}
