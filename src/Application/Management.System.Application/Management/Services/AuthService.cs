using Management.System.Common.Helpers;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Management.System.Common.Settings;
using Microsoft.Extensions.Options;

namespace Management.System.Application.Management.Services;

public class AuthService : IAuthService
{
    private readonly ICustomerService _customerService;
    private readonly IUserService _userService;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService
    (
        ICustomerService customerService,
        IUserService userService,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthService> logger
    )
    {
         _customerService = customerService;
        _userService = userService;
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public async Task<string> CreateAsync(string email, string password)
    {
        try
        {
            var user = await _userService.GetByEmailAsync(email);

            var customer = await _customerService.GetByEmailAsync(email);

            //var customer = await _customerRepository.GetByEmailAsync(email);

            if (user == null || !user.Password.Equals(password.CreateHash()))
            {
                throw new UnauthorizedAccessException("Os dados inseridos do usuário não correspondem aos registros existentes.");
            }

            if (customer == null)
            {
                throw new UnauthorizedAccessException("Os dados inseridos do cliente não correspondem aos registros existentes.");
            }

            var secretKey = _jwtSettings.Value.SecretKey;

            if (secretKey == null)
            {
                throw new Exception("Configuração de chave secreta não encontrada.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                    new Claim("userId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, customer.Name),
                    new Claim(ClaimTypes.Email, customer.Email),
                    new Claim("userType", user.UserType.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation("Token JWT criado com sucesso.");

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar token JWT.");
            throw;
        }
    }

    public string GetClaimValueFromToken(string token, string claimType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken!.ValidTo < DateTime.UtcNow)
        {
            throw new Exception("O token JWT expirou.");
        }

        var claims = jwtToken.Claims;

        var claim = claims.FirstOrDefault(c => c.Type == claimType);

        return claim!.Value;
    }
}
