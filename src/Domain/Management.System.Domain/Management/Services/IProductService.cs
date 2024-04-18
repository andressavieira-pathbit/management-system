using Management.System.Domain.Management.Services.Dtos;

namespace Management.System.Domain.Management.Services;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task CreateAsync(ProductDto productDto, UserDto user);
    Task UpdateAsync(Guid id, ProductDto productDto);
}
