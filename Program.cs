using LegacyOrderService.Data;
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;
using LegacyOrderService.Services;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");

            // Manually resolving dependencies, no DI container used
            IProductRepository productRepo = new ProductRepository();
            IOrderRepository orderRepo = new OrderRepository();
            IOrderService orderService = new OrderService(productRepo, orderRepo);

            string customerName;
            while (true)
            {
                Console.WriteLine("Enter customer name:");
                customerName = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(customerName))
                    break;
                Console.WriteLine("Error: Customer name cannot be empty.");
            }

            string productName;
            while (true)
            {
                Console.WriteLine("Enter product name:");
                productName = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(productName))
                {
                    break;
                }

                Console.WriteLine("Error: Product name cannot be empty.");
            }

            int qty;
            while (true)
            {
                Console.WriteLine("Enter quantity:");
                if (int.TryParse(Console.ReadLine(), out qty) && qty > 0)
                    break;
                Console.WriteLine("Error: Quantity must be a positive number.");
            }

            Console.WriteLine("Processing order...");

            var order = await orderService.PlaceOrderAsync(customerName, productName, qty);
            var orderReadModel = order.ToReadModel();
            Console.WriteLine("Order complete!");
            Console.WriteLine($"Customer: {orderReadModel.CustomerName}");
            Console.WriteLine($"Product: {orderReadModel.ProductName}");
            Console.WriteLine($"Quantity: {orderReadModel.Quantity}");
            Console.WriteLine($"Total: ${orderReadModel.Total}");
            Console.WriteLine("Done.");
        }
    }
}
