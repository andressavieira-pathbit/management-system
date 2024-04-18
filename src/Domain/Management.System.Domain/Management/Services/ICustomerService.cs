using Management.System.Domain.Management.Services.Dtos;

namespace Management.System.Domain.Management.Services;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<CustomerDto?> GetByEmailAsync(string email);
}
