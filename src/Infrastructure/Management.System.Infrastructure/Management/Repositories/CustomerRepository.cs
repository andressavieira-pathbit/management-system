using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Management.Repositories;

[ExcludeFromCodeCoverage]
public class CustomerRepository : ICustomerRepository
{
    private readonly Context _context;

    public CustomerRepository(Context context)
    {
        _context = context;
    }

    public async Task<CustomerEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<CustomerEntity?> GetByEmailAsync(string email)
    {

        return await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task CreateAsync(CustomerEntity customerEntity)
    {
        await _context.Customers.AddAsync(customerEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, CustomerEntity customerEntity)
    {
        var result = await _context.Customers.FindAsync(id);

        _context.Customers.Update(result!);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var result = await _context.Customers.FindAsync(id);

        _context.Customers.Remove(result!);
        await _context.SaveChangesAsync();
    }
}
