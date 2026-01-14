namespace LegacyOrderService.Exceptions
{
    public sealed class ProductNotFoundException : DomainException
    {
        public string ProductName { get; }

        public ProductNotFoundException(string productName)
            : base($"Product '{productName}' was not found.")
        {
            ProductName = productName;
        }
    }
}
