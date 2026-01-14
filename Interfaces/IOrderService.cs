using LegacyOrderService.Models;

namespace LegacyOrderService.Interfaces
{
    public interface IOrderService
    {
        Order PlaceOrder(string customerName, string productName, int quantity);
    }
}
