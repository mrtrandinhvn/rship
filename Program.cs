using LegacyOrderService.Data;
using LegacyOrderService.Models;

namespace LegacyOrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");

            string customerName;
            while (true)
            {
                Console.WriteLine("Enter customer name:");
                customerName = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(customerName))
                    break;
                Console.WriteLine("Error: Customer name cannot be empty.");
            }

            var productRepo = new ProductRepository();
            string productName;
            Product? product = null;
            while (true)
            {
                Console.WriteLine("Enter product name:");
                productName = Console.ReadLine() ?? ""; // a real system uses product IDs instead of names

                // validate input
                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Error: Product name cannot be empty.");
                    continue;
                }

                try
                {
                    product = productRepo.GetProduct(productName);
                    if (product != null)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error: Product '{productName}' not found. Please try again.");
                }
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

            Order order = new Order
            {
                CustomerName = customerName,
                ProductName = productName,
                Quantity = qty,
                Price = product.Price,
            };

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + order.ProductName);
            Console.WriteLine("Quantity: " + order.Quantity);
            Console.WriteLine("Total: $" + order.Total);

            Console.WriteLine("Saving order to database...");
            var repo = new OrderRepository();
            repo.Save(order);
            Console.WriteLine("Done.");
        }
    }
}
