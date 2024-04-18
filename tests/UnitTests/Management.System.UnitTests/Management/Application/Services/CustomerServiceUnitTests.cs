using AutoFixture;
using Management.System.Application.Management.Services;
using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Management.System.Domain.Management.Services;
using Moq;
using Xunit;

namespace Management.System.UnitTests.Management.Application.Services;

public class CustomerServiceUnitTests
{
    private readonly ICustomerService _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();

    public CustomerServiceUnitTests()
    {
        _sut = new CustomerService(_customerRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCustomerDto_WhenDataIsValid()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerEntity>();
        
        var user = _autoFixture.Create<UserEntity>();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customer.CustomerId)).ReturnsAsync(customer);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(customer.UserId)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetByIdAsync(customer.CustomerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.CustomerId, result!.CustomerId);
        Assert.Equal(user.UserId, result.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerEntity>();
        
        var user = _autoFixture.Create<UserEntity>();

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customer.CustomerId)).ReturnsAsync((CustomerEntity)null!);
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(customer.Email)).ReturnsAsync(user);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.GetByIdAsync(customer.CustomerId));

        Assert.NotNull(exception);
        Assert.Equal("Cliente não encontrado.", exception.Message);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsCustomerDto_WhenDataIsValid()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerEntity>();
      
        var user = _autoFixture.Create<UserEntity>();

        _customerRepositoryMock.Setup(r => r.GetByEmailAsync(customer.Email)).ReturnsAsync(customer);

        // Act
        var result = await _sut.GetByEmailAsync(customer.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.CustomerId, result!.CustomerId);
        Assert.Equal(customer.Email, result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var customer = _autoFixture.Create<CustomerEntity>();
      
        var user = _autoFixture.Create<UserEntity>();

        _customerRepositoryMock.Setup(r => r.GetByEmailAsync(customer.Email)).ReturnsAsync((CustomerEntity)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.GetByEmailAsync(customer.Email));
        
        Assert.NotNull(exception);
        Assert.Equal("Cliente não encontrado.", exception.Message);
    }
}
