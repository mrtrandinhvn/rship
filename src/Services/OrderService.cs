using LegacyOrderService.Events;
using LegacyOrderService.Exceptions;
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;
using Microsoft.Extensions.Logging;

namespace LegacyOrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IDomainEventDispatcher eventDispatcher,
            ILogger<OrderService> logger)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
        }

        public async Task<Order> PlaceOrderAsync(string customerName, string productName, int quantity)
        {
            _logger.LogInformation("Placing order for customer {CustomerName}, product {ProductName}, quantity {Quantity}",
                customerName, productName, quantity);

            var product = await _productRepository.GetProductByNameAsync(productName);
            if (product is null)
            {
                _logger.LogWarning("Order rejected: product {Product} not found", productName);
                throw new ProductNotFoundException(productName);
            }

            var order = Order.Create(customerName, productName, quantity, product.Price);
            await _orderRepository.SaveAsync(order);

            await _eventDispatcher.DispatchAsync(new OrderPlacedEvent(order.Id));
            _logger.LogInformation("Order placed successfully with total {Total}", order.Total);

            return order;
        }
    }
}
