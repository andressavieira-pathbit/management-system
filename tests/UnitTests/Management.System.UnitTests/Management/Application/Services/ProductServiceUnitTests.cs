using Management.System.Application.Management.Services;
using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Management.System.Domain.Management.Services.Dtos;
using Management.System.Domain.Management.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AutoFixture;
using Management.System.Domain.Management.Enums;

namespace Management.System.UnitTests.Management.Application.Services;

public class ProductServiceUnitTests
{
    private readonly IProductService _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<HttpClient> _httpClientMock = new();
    private readonly Mock<ILogger<ProductService>> _loggerMock = new();

    public ProductServiceUnitTests()
    {
        _sut = new ProductService(_authServiceMock.Object, _productRepositoryMock.Object, _httpClientMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsUserDto_WhenDataIsValid()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(product.ProductId)).ReturnsAsync(new ProductEntity { ProductId = product.ProductId });

        // Act
        var result = await _sut.GetByIdAsync(product.ProductId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.ProductId, result!.ProductId);
    }

    [Fact]
    public async Task GetByEmailAsync_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(product.ProductId)).ReturnsAsync((ProductEntity)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.GetByIdAsync(product.ProductId));

        Assert.NotNull(exception);
        Assert.Equal("Produto não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();
      
        var user = _autoFixture.Create<UserDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(product.AccessToken, "userType")).Returns(EUserType.Admin.ToString());

        // Act
        await _sut.CreateAsync(product, user);

        _productRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<ProductEntity>())); 
    }

    [Fact]
    public async Task CreateAsync_ThrowsUnauthorizedAccessException_WhenUserTypeNotFound()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();
       
        var user = _autoFixture.Create<UserDto>();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(product, user));

        Assert.NotNull(exception);
        Assert.Equal("Tipo de usuário não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsUnauthorizedAccessException_WhenUnauthorizedUser()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();
        product.Quantity = 10;

        var user = _autoFixture.Create<UserDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(product.AccessToken, "userType")).Returns(EUserType.Client.ToString());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(product, user));

        Assert.NotNull(exception);
        Assert.Equal("Acesso não autorizado para usuários que não são administradores.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenDataIsInvalid()
    {
        var product = _autoFixture.Create<ProductDto>();
        product.ProductId = Guid.NewGuid();
        product.Quantity = -1;

        var user = _autoFixture.Create<UserDto>();

        _authServiceMock.Setup(a => a.GetClaimValueFromToken(product.AccessToken, "userType")).Returns(EUserType.Admin.ToString());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAsync(product, user));

        Assert.NotNull(exception);
        Assert.Equal("Produto inválido.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenDataIsValid()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();

        _productRepositoryMock.Setup(r => r.GetByIdAsync(product.ProductId)).ReturnsAsync(new ProductEntity { ProductId = product.ProductId });

        // Act
        await _sut.UpdateAsync(product.ProductId, product);

        // Assert
        _productRepositoryMock.Verify(r => r.UpdateAsync(product.ProductId, It.IsAny<ProductEntity>()));

        Assert.NotNull(product);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var product = _autoFixture.Create<ProductDto>();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.UpdateAsync(product.ProductId, product));

        Assert.NotNull(exception);
        Assert.Equal("Produto não encontrado.", exception.Message);
    }
}
