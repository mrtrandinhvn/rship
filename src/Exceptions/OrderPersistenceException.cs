namespace LegacyOrderService.Exceptions
{
    public class OrderPersistenceException : DomainException
    {
        public OrderPersistenceException(string message) : base(message) { }
    }
}
