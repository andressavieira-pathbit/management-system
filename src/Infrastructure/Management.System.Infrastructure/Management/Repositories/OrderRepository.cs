using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Management.Repositories;

[ExcludeFromCodeCoverage]
public class OrderRepository : IOrderRepository
{
    private readonly Context _context;

    public OrderRepository(Context context)
    {
        _context = context;
    }

    public async Task<OrderEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task CreateAsync(OrderEntity orderEntity)
    {
        await _context.Orders.AddAsync(orderEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, OrderEntity orderEntity)
    {
        var result = await _context.Products.FindAsync(id);

        _context.Products.Update(result!);
        await _context.SaveChangesAsync();
    }
}
