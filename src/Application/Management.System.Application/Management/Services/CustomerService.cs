using Management.System.Application.Management.Mappers;
using Management.System.Domain.Management.Services;
using Management.System.Domain.Management.Services.Dtos;
using Management.System.Domain.Management.Repositories;
using Microsoft.Extensions.Logging;

namespace Management.System.Application.Management.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;

    public CustomerService
    (
        ICustomerRepository customerRepository,
        IUserRepository userRepository
    )
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var result = await _customerRepository.GetByIdAsync(id);

        if (result == null)
        {
            throw new Exception("Cliente não encontrado.");
        }

        var user = await _userRepository.GetByIdAsync(result.UserId); 

        result.UserId = user!.UserId;

        return result.Map();
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        var result = await _customerRepository.GetByEmailAsync(email);

        if (result == null)
        {
            throw new Exception("Cliente não encontrado.");
        }

        var response = result.Map();

        return response;
    }
}
