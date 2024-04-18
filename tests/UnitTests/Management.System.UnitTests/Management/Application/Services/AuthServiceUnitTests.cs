using Management.System.Application.Management.Services;
using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Management.System.Domain.Management.Services.Dtos;
using Management.System.Domain.Management.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Management.System.Common.Helpers;
using Management.System.Domain.Management.Enums;
using AutoFixture;
using Management.System.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Management.System.UnitTests.Management.Application.Services;

public class AuthServiceUnitTests
{
    private readonly Fixture _autoFixture = new();
    private readonly Mock<ICustomerService> _customerServiceMock = new();
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock = new();
    private readonly Mock<ILogger<AuthService>> _loggerMock = new();

    public AuthServiceUnitTests() 
    {
    } 

    [Fact]
    public async Task CreateAsync_ReturnsJwtToken_WhenDataIsValid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password_123";
        var customerId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var user = new UserDto
        {
            UserId = userId,
            Password = password.CreateHash(),
            UserType = EUserType.Client
        };

        var customer = new CustomerDto
        {
            CustomerId = customerId,
            Name = "Test",
            Email = email
        };

        _userServiceMock.Setup(u => u.GetByEmailAsync(email)).ReturnsAsync(user);
        //_customerRepositoryMock.Setup(m => m.GetByEmailAsync(customer.Email)).ReturnsAsync(new CustomerEntity { CustomerId = customerId.CustomerId, Name = customer.Name, Email = customer.Email, UserId = customer.UserId });
        _customerServiceMock.Setup(c => c.GetByEmailAsync(email)).ReturnsAsync(new CustomerDto { CustomerId = customer.CustomerId, Name = customer.Name, Email = customer.Email, UserId = user.UserId }); ;

        var settings = new JwtSettings { SecretKey = "6EA2A01EB704C7F0337F4FAC7C1B3F5B" };
        var options = Options.Create(settings);

        _jwtSettingsMock.Setup(x => x.Value).Returns(options.Value);

        var _sut = new AuthService(_customerServiceMock.Object, _userServiceMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);

        // Act
        var token = await _sut.CreateAsync(email, password);

        // Assert
        Assert.NotNull(token);
    }

    [Fact]
    public async Task CreateAsync_ReturnsUnauthorizedAccessException_WhenUserUnauthorized()
    {
        // Arrange
        var user = _autoFixture.Create<UserDto>();

        _userServiceMock.Setup(u => u.GetByEmailAsync(user.Email)).ReturnsAsync(new UserDto { UserId = user.UserId, Email = user.Email, Password = user.Password, UserName = user.UserName, UserType = user.UserType });

        var _sut = new AuthService(_customerServiceMock.Object, _userServiceMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(user.Email, user.Password));

        Assert.NotNull(exception);
        Assert.Equal("Os dados inseridos do usuário não correspondem aos registros existentes.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ReturnsUnauthorizedAccessException_WhenCustomerUnauthorized()
    {
        // Arrange
        var user = _autoFixture.Create<UserDto>();
        var customer = _autoFixture.Create<CustomerDto>();

        _userServiceMock.Setup(u => u.GetByEmailAsync(user.Email)).ReturnsAsync(new UserDto { UserId = user.UserId, Email = user.Email, Password = user.Password.CreateHash(), UserName = user.UserName, UserType = user.UserType });
          _customerServiceMock.Setup(m => m.GetByEmailAsync(customer.Email)).ReturnsAsync(new CustomerDto { CustomerId = customer.CustomerId, Name = customer.Name, Email = customer.Email, UserId = customer.UserId }); 

        var _sut = new AuthService(_customerServiceMock.Object, _userServiceMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _sut.CreateAsync(user.Email, user.Password));

        Assert.NotNull(exception);
        Assert.Equal("Os dados inseridos do cliente não correspondem aos registros existentes.", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ReturnsException_WhenDataIsInvalid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password_123";
        var customerId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var user = new UserDto
        {
            UserId = userId,
            Password = password.CreateHash(),
            UserType = EUserType.Client
        };

        var customer = new CustomerDto
        {
            CustomerId = customerId,
            Name = "Test",
            Email = email
        };

        _userServiceMock.Setup(u => u.GetByEmailAsync(email)).ReturnsAsync(user);
        _customerServiceMock.Setup(c => c.GetByEmailAsync(email)).ReturnsAsync(new CustomerDto { CustomerId = customerId, Name = customer.Name, Email = email, UserId = userId });

        var settings = new JwtSettings { SecretKey = null! };
        var options = Options.Create(settings);

        _jwtSettingsMock.Setup(x => x.Value).Returns(options.Value);

        var _sut = new AuthService(_customerServiceMock.Object, _userServiceMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateAsync(email, password));
        
        Assert.NotNull(exception);
        Assert.Equal("Configuração de chave secreta não encontrada.", exception.Message);
    }

    [Fact]
    public void GetClaimValueFromToken_ReturnsClaimValue_WhenClaimExists()
    {
        // Arrange
        var settings = new JwtSettings { SecretKey = "6EA2A01EB704C7F0337F4FAC7C1B3F5B" };
        var options = Options.Create(settings);

        var secretKey = options.Value.SecretKey;

        var key = Encoding.UTF8.GetBytes(secretKey);

        var securityKey = new SymmetricSecurityKey(key);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>
        {
            new Claim("email", "john@example.com") 
        };

        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            Subject = principal.Identity as ClaimsIdentity,
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = signingCredentials
        });

        var _sut = new AuthService(_customerServiceMock.Object, _userServiceMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);

        // Act
        var claimValue = _sut.GetClaimValueFromToken(token, "email"); 

        // Assert
        Assert.NotNull(claimValue);
        Assert.Equal("john@example.com", claimValue);
    }

    [Fact]
    public async Task GetClaimValueFromToken_ReturnsException_WhenTokenExpired()
    {
        // Arrange
        var settings = new JwtSettings { SecretKey = "6EA2A01EB704C7F0337F4FAC7C1B3F5B" };
        var options = Options.Create(settings);

        var secretKey = options.Value.SecretKey;

        var key = Encoding.UTF8.GetBytes(secretKey);

        var securityKey = new SymmetricSecurityKey(key);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var now = DateTime.UtcNow;
        var claims = new List<Claim>
        {
            new Claim("email", "john@example.com")
        };

        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            Subject = principal.Identity as ClaimsIdentity,
            Expires = DateTime.UtcNow.AddSeconds(1),
            SigningCredentials = signingCredentials
        });

        await Task.Delay(1001);

        var _sut = new AuthService(_customerServiceMock.Object, _userServiceMock.Object, _jwtSettingsMock.Object, _loggerMock.Object);

        // Act e Assert
        var exception = Assert.Throws<Exception>(() => _sut.GetClaimValueFromToken(token, "email"));

        Assert.NotNull(exception);
        Assert.Equal("O token JWT expirou.", exception.Message);
    }
}
