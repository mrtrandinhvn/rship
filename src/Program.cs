using LegacyOrderService.Data;
using LegacyOrderService.Exceptions;
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;
using LegacyOrderService.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");

            // Manually resolving dependencies, no DI container used
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(Log.Logger);
            });

            var logger = loggerFactory.CreateLogger<OrderService>();
            IDistributedCache cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            IProductRepository productRepo = new CachingProductRepository(new ProductRepository(), cache);
            IOrderRepository orderRepo = new OrderRepository($"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}");
            IOrderService orderService = new OrderService(productRepo, orderRepo, logger);

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

            try
            {
                var order = await orderService.PlaceOrderAsync(customerName, productName, qty);
                var orderReadModel = order.ToReadModel();
                Console.WriteLine("Order complete!");
                Console.WriteLine($"Customer: {orderReadModel.CustomerName}");
                Console.WriteLine($"Product: {orderReadModel.ProductName}");
                Console.WriteLine($"Quantity: {orderReadModel.Quantity}");
                Console.WriteLine($"Total: ${orderReadModel.Total}");
            }
            catch (DomainException ex)
            {
                logger.LogError(ex, "Domain Exception: ");
            }

            Console.WriteLine("Done.");
        }
    }
}
