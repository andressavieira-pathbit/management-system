using Management.System.Domain.Management.Entities;

namespace Management.System.Domain.Management.Repositories;

public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
{
    Task<CustomerEntity?> GetByEmailAsync(string email);
}
