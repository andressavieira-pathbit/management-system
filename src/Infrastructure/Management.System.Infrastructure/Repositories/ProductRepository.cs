using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Repositories;
using Management.System.Infrastructure.Repository;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class ProductRepository : IProductRepository
{
    private readonly Context _context;

    public ProductRepository(Context context)
    {
        _context = context;
    }

    public async Task<ProductEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task CreateAsync(ProductEntity productEntity)
    {
        await _context.Products.AddAsync(productEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, ProductEntity productEntity)
    {
        var result = await _context.Products.FindAsync(id);

        _context.Products.Update(result!);
        await _context.SaveChangesAsync();
    }
}
