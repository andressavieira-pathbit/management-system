using Management.System.Common.Helpers;
using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Management.System.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class UserRepository : IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context)
    {
        _context = context;
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
       return await _context.Users.FindAsync(id);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task CreateAsync(UserEntity userEntity)
    {
        userEntity.Password = userEntity.Password.CreateHash();

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UserEntity userEntity)
    {
        var result = await _context.Users.FindAsync(id);

        _context.Users.Update(result!);
        await _context.SaveChangesAsync();
    }
}
