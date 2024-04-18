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

public class AuthControllerUnitTests
{
    private readonly AuthController _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<ILogger<AuthController>> _loggerMock = new();

    public AuthControllerUnitTests()
    {
        _sut = new AuthController(_userServiceMock.Object, _authServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsCreated_WhenDataIsValid()
    {
        // Arrange
        var request = _autoFixture.Create<LoginRequest>();
        var user = _autoFixture.Create<UserDto>();
        var token = string.Empty;
        
        _userServiceMock
              .Setup(it => it.GetByEmailAsync(request.Email))
              .ReturnsAsync(user);

        _authServiceMock
               .Setup(it => it.CreateAsync(user.Email, request.Password))
               .ReturnsAsync(token);

        // Act
        var response = await _sut.HandleAsync(request);

        // Assert
        var actionResult = Assert.IsType<CreatedResult>(response);

        Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);
    }

    [Fact]
    public async Task HandleAsync_ReturnsBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var request = _autoFixture.Create<LoginRequest>();
        var user = _autoFixture.Create<UserDto>();

        _userServiceMock
           .Setup(it => it.GetByEmailAsync(request.Email))
           .ThrowsAsync(new Exception("Usuário não encontrado."));

        _authServiceMock
            .Setup(it => it.CreateAsync(request.Email, request.Password))
            .ThrowsAsync(new Exception("Erro ao criar token."));

        // Act
        var response = await _sut.HandleAsync(request);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(response);

        Assert.Equal(StatusCodes.Status400BadRequest, actionResult.StatusCode);
    }
}
