using Microsoft.Extensions.Logging;
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;

namespace LegacyOrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order> PlaceOrderAsync(string customerName, string productName, int quantity)
        {
            _logger.LogInformation("Placing order for customer {CustomerName}, product {ProductName}, quantity {Quantity}",
                customerName, productName, quantity);

            var product = await _productRepository.GetProductByNameAsync(productName)
                ?? throw new ArgumentException($"Product '{productName}' not found.", nameof(productName));
            var order = Order.Create(customerName, productName, quantity, product.Price);
            await _orderRepository.SaveAsync(order);

            _logger.LogInformation("Order placed successfully with total {Total}", order.Quantity * order.Price);

            return order;
        }
    }
}
