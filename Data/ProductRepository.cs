// Data/ProductRepository.cs
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;

namespace LegacyOrderService.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Dictionary<string, Product> _products = new()
        {
            ["Widget"] = new Product { Id = 1, Name = "Widget", Price = 12.99 },
            ["Gadget"] = new Product { Id = 2, Name = "Gadget", Price = 15.49 },
            ["Doohickey"] = new Product { Id = 3, Name = "Doohickey", Price = 8.75 }
        };

        /// <summary>
        /// Retrieves the product with the specified name from the collection.
        /// </summary>
        /// <remarks>This method performs a potentially time-consuming lookup operation. Consider caching
        /// results if calling frequently.</remarks>
        /// <param name="productName">The name of the product to retrieve. Cannot be null or empty.</param>
        /// <returns>The <see cref="Product"/> instance that matches the specified name.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a product with the specified name does not exist in the collection.</exception>
        public async Task<Product?> GetProductByNameAsync(string productName)
        {
            // Simulate an expensive async lookup
            await Task.Delay(500);

            if (_products.TryGetValue(productName, out var product))
                return product;

            throw new KeyNotFoundException($"Product '{productName}' not found");
        }
    }
}
