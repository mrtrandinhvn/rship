using LegacyOrderService.Interfaces;
using Microsoft.Extensions.Logging;

namespace LegacyOrderService.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger)
        {
            _logger = logger;
        }

        public Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : class
        {
            _logger.LogInformation("Domain event dispatched: {EventType} - {@Event}",
                typeof(TEvent).Name, domainEvent);

            // Future: add actual event dispatching logic here (e.g., message bus)
            return Task.CompletedTask;
        }
    }
}
