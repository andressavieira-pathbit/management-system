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

public class OrderControllerUnitTests
{
    private readonly OrderController _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IOrderService> _orderServiceMock = new();
    private readonly Mock<ILogger<OrderController>> _loggerMock = new();

    public OrderControllerUnitTests()
    {
        _sut = new OrderController(_authServiceMock.Object, _orderServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsCreated_WhenIsValid()
    {
        // Arrange
        var request = _autoFixture.Create<OrderRequest>();
        var accessToken = string.Empty;
        var email = string.Empty;

        _authServiceMock
            .SetupSequence(it => it.GetClaimValueFromToken(accessToken, email))
            .Returns(string.Empty);

        var customer = _autoFixture.Create<CustomerDto>();
        var result = _autoFixture.Create<OrderDto>();

        _orderServiceMock
            .Setup(it => it.CreateAsync(result, new CustomerDto()
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
        var request = _autoFixture.Create<OrderRequest>();
        var email = string.Empty;

        var customer = _autoFixture.Create<CustomerDto>();
        var result = _autoFixture.Create<OrderDto>();

        _orderServiceMock
            .Setup(it => it.CreateAsync(result, new CustomerDto()
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
