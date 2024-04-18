using AutoFixture;
using Management.System.Application.Management.Services;
using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Management.System.UnitTests.Management.Application.Services;

public class UserServiceUnitTests
{
    private readonly IUserService _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ILogger<UserService>> _loggerMock = new();

    public UserServiceUnitTests()
    {
        _sut = new UserService(_userRepositoryMock.Object, _customerRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsUserDto_WhenDataIsValid()
    {
        // Arrange
        var user = _autoFixture.Create<UserEntity>();

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetByEmailAsync(user.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result!.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var user = _autoFixture.Create<UserEntity>();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.GetByEmailAsync(user.Email));

        Assert.NotNull(exception);
        Assert.Equal("Usuário não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid()
    {
        // Arrange
        var user = _autoFixture.Create<UserDto>();
     
        var customerDto = _autoFixture.Create<CustomerDto>();

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync((UserEntity)null!);

        // Act
        await _sut.CreateAsync(user, customerDto);

        // Assert
        _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<UserEntity>()));
        _customerRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<CustomerEntity>()));
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenDataIsInvalid()
    {
     
        // Arrange
        var user = _autoFixture.Create<UserDto>();
        var existingUser = _autoFixture.Create<UserEntity>();

        var customer = _autoFixture.Create<CustomerDto>();

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAsync(user, customer));

        Assert.NotNull(exception);
        Assert.Equal("Usuário já cadastrado.", exception.Message);
    }
}
