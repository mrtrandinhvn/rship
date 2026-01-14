namespace LegacyOrderService.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : class;
    }
}
