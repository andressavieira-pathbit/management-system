using AutoFixture;
using Management.System.Application.Management.Services;
using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Enums;
using Management.System.Domain.Management.ExternalApis;
using Management.System.Domain.Management.ExternalApis.Dtos;
using Management.System.Domain.Management.Repositories;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Management.System.UnitTests.Management.Application.Services;

public class OrderServiceUnitTests
{

    private readonly IOrderService _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<ICustomerService> _customerServiceMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<IZipCodeService> _zipCodeServiceMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<HttpClient> _httpClientMock = new();
    private readonly Mock<ILogger<OrderService>> _loggerMock = new();

    public OrderServiceUnitTests()
    {
        _sut = new OrderService(_authServiceMock.Object, _customerServiceMock.Object, _productServiceMock.Object, _zipCodeServiceMock.Object, _orderRepositoryMock.Object, _unitOfWorkMock.Object, _httpClientMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var product = _autoFixture.Create<ProductDto>();
        product.ProductId = Guid.NewGuid();
        product.Quantity = 10;

        var order = _autoFixture.Create<OrderDto>();
        order.ProductId = product.ProductId;
        order.Quantity = 9;

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "nameid")).Returns(customer.CustomerId.ToString());

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userType")).Returns(EUserType.Client.ToString());

        var zipCode = new List<ZipCodeDto>();

        _zipCodeServiceMock.Setup(a => a.GetAsync(order.ZipCode)).ReturnsAsync(new List<ZipCodeDto>());

        _productServiceMock.Setup(a => a.GetByIdAsync(product.ProductId)).ReturnsAsync(product);

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userId")).Returns(customer.UserId.ToString());

        _customerServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

        var transactionMock = new Mock<IDbContextTransaction>();
        
        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        _orderRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<OrderEntity>())).Returns(Task.CompletedTask);

        // Act
        await _sut.CreateAsync(order, customer);

        // Assert
        _orderRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<OrderEntity>()), Times.Once);
        
        transactionMock.Verify(t => t.Commit(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenIdNotFound()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var order = _autoFixture.Create<OrderDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userType")).Returns(EUserType.Admin.ToString());

        var transactionMock = new Mock<IDbContextTransaction>();

        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(order, customer));

        Assert.NotNull(exception);
        Assert.Equal("ID do usuário não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenUserTypeNotFound()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var order = _autoFixture.Create<OrderDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "nameid")).Returns(customer.CustomerId.ToString());

        var transactionMock = new Mock<IDbContextTransaction>();

        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(order, customer));

        Assert.NotNull(exception);
        Assert.Equal("Tipo de usuário não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenUnauthorizedUser()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var order = _autoFixture.Create<OrderDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "nameid")).Returns(customer.CustomerId.ToString());

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userType")).Returns(EUserType.Admin.ToString());

        var transactionMock = new Mock<IDbContextTransaction>();

        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(order, customer));

        Assert.NotNull(exception);
        Assert.Equal("Acesso não autorizado para usuários que não são clientes.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenZipCodeNotFound()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var product = _autoFixture.Create<ProductDto>();

        var order = _autoFixture.Create<OrderDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "nameid")).Returns(customer.CustomerId.ToString());

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userType")).Returns(EUserType.Client.ToString());

        _orderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrderEntity>())).Returns(Task.CompletedTask);

        var transactionMock = new Mock<IDbContextTransaction>();

        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAsync(order, customer));

        transactionMock.Verify(t => t.Rollback(), Times.Once);
        Assert.NotNull(exception);
        Assert.Equal("Endereço não encontrado para o CEP fornecido.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenProductNotFound()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var product = _autoFixture.Create<ProductDto>();

        var order = _autoFixture.Create<OrderDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "nameid")).Returns(customer.CustomerId.ToString());

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userType")).Returns(EUserType.Client.ToString());

        _zipCodeServiceMock.Setup(a => a.GetAsync(order.ZipCode)).ReturnsAsync(new List<ZipCodeDto>());

        _orderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrderEntity>())).Returns(Task.CompletedTask);

        var transactionMock = new Mock<IDbContextTransaction>();

        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAsync(order, customer));

        transactionMock.Verify(t => t.Rollback(), Times.Once);
        
        Assert.NotNull(exception);
        Assert.Equal("Produto informado não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenQuantityUnavailable()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerDto>();

        var product = _autoFixture.Create<ProductDto>();
        product.ProductId = Guid.NewGuid();
        product.Quantity = 10;

        var order = _autoFixture.Create<OrderDto>();
        order.ProductId = product.ProductId;
        order.Quantity = 11;

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "nameid")).Returns(customer.CustomerId.ToString());

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userType")).Returns(EUserType.Client.ToString());

        var zipCode = new List<ZipCodeDto>();

        _zipCodeServiceMock.Setup(a => a.GetAsync(order.ZipCode)).ReturnsAsync(new List<ZipCodeDto>());

        _productServiceMock.Setup(a => a.GetByIdAsync(product.ProductId)).ReturnsAsync(product);

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(order.AccessToken, "userId")).Returns(customer.UserId.ToString());

        _customerServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

        var transactionMock = new Mock<IDbContextTransaction>();

        _unitOfWorkMock.Setup(u => u.BeginTransaction()).Returns(transactionMock.Object);

        _orderRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<OrderEntity>())).Returns(Task.CompletedTask);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAsync(order, customer));

        transactionMock.Verify(t => t.Rollback(), Times.Once);

        Assert.NotNull(exception);
        Assert.Equal("Quantidade disponível do produto insuficiente para atender à ordem.", exception.Message);
    }
}
