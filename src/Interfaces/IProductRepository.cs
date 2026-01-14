using LegacyOrderService.Models;

namespace LegacyOrderService.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a product that matches the specified name.
        /// </summary>
        /// <param name="productName">The name of the product to search for. Cannot be null or empty.</param>
        /// <returns>A <see cref="Product"/> instance that matches the specified name, or <see langword="null"/> if no matching
        /// product is found.</returns>
        Task<Product?> GetProductByNameAsync(string productName);
    }
}
