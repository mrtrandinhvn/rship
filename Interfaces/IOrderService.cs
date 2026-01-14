using LegacyOrderService.Models;

namespace LegacyOrderService.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(string customerName, string productName, int quantity);
    }
}
