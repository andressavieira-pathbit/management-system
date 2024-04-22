using Management.System.Domain.Management.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Management.Repositories;

[ExcludeFromCodeCoverage]
public class RepositoryBase<EntityT> : IRepositoryBase<EntityT> where EntityT : class
{
    private readonly Context _context;
    private readonly DbSet<EntityT> _dbSet;

    public RepositoryBase(Context context)
    {
        _context = context;
        _dbSet = _context.Set<EntityT>();
    }

    public async Task<EntityT?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task CreateAsync(EntityT entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, EntityT entity)
    {
        var result = await _dbSet.FindAsync(id);

        _dbSet.Update(result!);
        await _context.SaveChangesAsync();
    }
}
