using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LegacyOrderService.Data
{
    public class CachingProductRepository : IProductRepository
    {
        private readonly IProductRepository _inner;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };

        public CachingProductRepository(IProductRepository inner, IDistributedCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<Product?> GetProductByNameAsync(string productName)
        {
            var cacheKey = $"product:{productName}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached is not null)
                return JsonSerializer.Deserialize<Product>(cached);

            var product = await _inner.GetProductByNameAsync(productName);
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product), _cacheOptions);
            return product;
        }
    }
}
