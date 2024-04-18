namespace Management.System.Domain.Management.Repositories;

public interface IRepositoryBase<EntityT> where EntityT : class
{
    Task<EntityT?> GetByIdAsync(Guid id);
    Task CreateAsync(EntityT entity);
    Task UpdateAsync(Guid Id, EntityT entity);
}
