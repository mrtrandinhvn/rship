using LegacyOrderService.Models;

namespace LegacyOrderService.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Saves the specified order to the data store.
        /// </summary>
        /// <param name="order">The order to be saved. Cannot be null.</param>
        Task SaveAsync(Order order);
    }
}
