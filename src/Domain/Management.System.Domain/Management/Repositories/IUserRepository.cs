using Management.System.Domain.Management.Entities;

namespace Management.System.Domain.Management.Repositories;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email);
}
