using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;

namespace LegacyOrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public Order PlaceOrder(string customerName, string productName, int quantity)
        {
            var product = _productRepository.GetProductByName(productName) ?? throw new ArgumentException($"Product '{productName}' not found.", nameof(productName));
            var order = Order.Create(customerName, productName, quantity, product.Price);
            _orderRepository.Save(order);
            return order;
        }
    }
}
