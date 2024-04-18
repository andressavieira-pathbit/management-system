using AutoFixture;
using Management.System.Api.Management.Controllers;
using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Management.System.UnitTests.Management.Api.Controllers;

public class ProdutControllerUnitTests
{
    private readonly ProductController _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<ILogger<ProductController>> _loggerMock = new();

    public ProdutControllerUnitTests()
    {
        _sut = new ProductController(_authServiceMock.Object, _productServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsCreated_WhenIsValid()
    {
        // Arrange
        var request = _autoFixture.Create<ProductRequest>();
        var accessToken = string.Empty;
        var email = string.Empty;

        _authServiceMock
            .SetupSequence(it => it.GetClaimValueFromToken(accessToken, email))
            .Returns(string.Empty);

        var user = _autoFixture.Create<UserDto>();
        var result = _autoFixture.Create<ProductDto>();

        _productServiceMock
            .Setup(it => it.CreateAsync(result, new UserDto()
            {
                Email = email
            }))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _sut.HandleAsync(request, accessToken);

        // Assert
        var actionResult = Assert.IsType<CreatedResult>(response);

        Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);
    }

    [Fact]
    public async Task HandleAsync_ReturnsBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var request = _autoFixture.Create<ProductRequest>();
        var email = string.Empty;

        var user = _autoFixture.Create<UserDto>();
        var result = _autoFixture.Create<ProductDto>();

        _productServiceMock
            .Setup(it => it.CreateAsync(result, new UserDto()
            {
                Email = email
            }))
            .ThrowsAsync(new Exception("Erro ao criar produto."));

        // Act
        var response = await _sut.HandleAsync(null!, null!);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(response);

        Assert.Equal(StatusCodes.Status400BadRequest, actionResult.StatusCode);
    }
}
