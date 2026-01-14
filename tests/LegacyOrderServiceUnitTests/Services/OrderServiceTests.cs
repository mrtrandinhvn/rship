using LegacyOrderService.Events;
using LegacyOrderService.Exceptions;
using LegacyOrderService.Interfaces;
using LegacyOrderService.Models;
using LegacyOrderService.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace LegacyOrderServiceUnitTests.Services;

public class OrderServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IDomainEventDispatcher> _eventDispatcherMock;
    private readonly Mock<ILogger<OrderService>> _loggerMock;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _eventDispatcherMock = new Mock<IDomainEventDispatcher>();
        _loggerMock = new Mock<ILogger<OrderService>>();
        _service = new OrderService(
            _productRepositoryMock.Object,
            _orderRepositoryMock.Object,
            _eventDispatcherMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task PlaceOrderAsync_WithValidProduct_ReturnsOrder()
    {
        // Arrange
        var product = new Product { Name = "Widget", Price = 9.99m };
        _productRepositoryMock
            .Setup(x => x.GetProductByNameAsync("Widget"))
            .ReturnsAsync(product);

        // Act
        var result = await _service.PlaceOrderAsync("John Doe", "Widget", 2);

        // Assert
        Assert.Equal("John Doe", result.CustomerName);
        Assert.Equal("Widget", result.ProductName);
        Assert.Equal(2, result.Quantity);
        Assert.Equal(9.99m, result.Price);
    }

    [Fact]
    public async Task PlaceOrderAsync_WithValidProduct_SavesOrder()
    {
        // Arrange
        const string CustomerName = "John Doe";
        const string ProductName = "Widget";
        var product = new Product { Name = ProductName, Price = 9.99m };
        _productRepositoryMock
            .Setup(x => x.GetProductByNameAsync(ProductName))
            .ReturnsAsync(product);

        // Act
        await _service.PlaceOrderAsync(CustomerName, ProductName, 2);

        // Assert
        _orderRepositoryMock.Verify(x => x.SaveAsync(It.Is<Order>(o =>
            o.CustomerName == CustomerName &&
            o.ProductName == ProductName &&
            o.Quantity == 2 &&
            o.Price == 9.99m)), Times.Once);
    }

    [Fact]
    public async Task PlaceOrderAsync_WithNonExistentProduct_ThrowsProductNotFoundException()
    {
        // Arrange
        const string NonExistentProductName = "NonExistent";
        _productRepositoryMock
            .Setup(x => x.GetProductByNameAsync(NonExistentProductName))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProductNotFoundException>(
            () => _service.PlaceOrderAsync("John Doe", NonExistentProductName, 1));

        Assert.Equal(exception.ProductName, NonExistentProductName);
    }

    [Fact]
    public async Task PlaceOrderAsync_WithNonExistentProduct_DoesNotSaveOrder()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.GetProductByNameAsync("NonExistent"))
            .ReturnsAsync((Product?)null);

        // Act
        try
        {
            await _service.PlaceOrderAsync("John Doe", "NonExistent", 1);
        }
        catch (ProductNotFoundException)
        {
            // Expected
        }

        // Assert
        _orderRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task PlaceOrderAsync_WithValidProduct_DispatchesOrderPlacedEvent()
    {
        // Arrange
        const string CustomerName = "John Doe";
        const string ProductName = "Widget";
        const int Quantity = 2;
        const decimal Price = 9.99m;
        var product = new Product { Name = ProductName, Price = Price };
        _productRepositoryMock
            .Setup(x => x.GetProductByNameAsync(ProductName))
            .ReturnsAsync(product);

        // Act
        await _service.PlaceOrderAsync(CustomerName, ProductName, Quantity);

        // Assert
        _orderRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Order>()), Times.Once);
        _eventDispatcherMock.Verify(x => x.DispatchAsync(It.IsAny<OrderPlacedEvent>()), Times.Once);
    }

    [Fact]
    public async Task PlaceOrderAsync_WithNonExistentProduct_DoesNotDispatchEvent()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.GetProductByNameAsync("NonExistent"))
            .ReturnsAsync((Product?)null);

        // Act
        try
        {
            await _service.PlaceOrderAsync("John Doe", "NonExistent", 1);
        }
        catch (ProductNotFoundException)
        {
            // Expected
        }

        // Assert
        _eventDispatcherMock.Verify(x => x.DispatchAsync(It.IsAny<OrderPlacedEvent>()), Times.Never);
        _orderRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Order>()), Times.Never);
    }
}
