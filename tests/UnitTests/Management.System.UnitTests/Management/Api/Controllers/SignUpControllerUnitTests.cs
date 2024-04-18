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

public class SignUpControllerUnitTests
{
    private readonly SignUpController _sut;
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<ILogger<SignUpController>> _loggerMock = new();

    public SignUpControllerUnitTests()
    {
        _sut = new SignUpController(_userServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsCreated_WhenDataIsValid()
    {
        // Arrange
        var request = _autoFixture.Create<SignUpRequest>();

        _userServiceMock
            .SetupSequence(it => it.CreateAsync(It.IsAny<UserDto>(), It.IsAny<CustomerDto>()))
            .Returns(Task.CompletedTask);

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
        var request = _autoFixture.Create<SignUpRequest>();

        _userServiceMock
            .Setup(it => it.CreateAsync(It.IsAny<UserDto>(), It.IsAny<CustomerDto>()))
            .ThrowsAsync(new Exception("Erro ao criar usuário"));

        // Act
        var response = await _sut.HandleAsync(request);

        // Assert
        var actionResult = Assert.IsType<BadRequestObjectResult>(response);

        Assert.Equal(StatusCodes.Status400BadRequest, actionResult.StatusCode);
    }
}
