using Management.System.Domain.Management.Services.Dtos;

namespace Management.System.Domain.Management.Services;

public interface IUserService
{
    Task<UserDto?> GetByEmailAsync(string email);
    Task CreateAsync(UserDto userDto, CustomerDto customerDto);
}
