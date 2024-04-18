using Management.System.Application.Management.Mappers;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Management.System.Domain.Management.Repositories;
using Microsoft.Extensions.Logging;

namespace Management.System.Application.Management.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<UserService> _logger;

    public UserService
    (
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        ILogger<UserService> logger
    )
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var result = await _userRepository.GetByEmailAsync(email);

        if (result == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        var response = result.Map();

        return response;
    }

    public async Task CreateAsync(UserDto userDto, CustomerDto customerDto)
    {
        try
        {
            var userExists = await _userRepository.GetByEmailAsync(userDto.Email);

            if (userExists != null || userExists?.Email != null || userExists?.Email == userDto.Email)
            {
                throw new Exception("Usuário já cadastrado.");
            }

            var user = userDto.Map();

            await _userRepository.CreateAsync(user);

            customerDto = new CustomerDto()
            {
                Name = customerDto.Name,
                Email = userDto.Email,
                UserId = user.UserId,
            };

            var customer = customerDto.Map();

            await _customerRepository.CreateAsync(customer); 

            _logger.LogInformation("Informações de registro criadas com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário.");
            throw;
        }
    }
}
