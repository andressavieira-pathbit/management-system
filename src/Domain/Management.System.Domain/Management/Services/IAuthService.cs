namespace Management.System.Domain.Management.Services;

public interface IAuthService
{
    Task<string> CreateAsync(string email, string password);
    string GetClaimValueFromToken(string token, string claimType);
}
