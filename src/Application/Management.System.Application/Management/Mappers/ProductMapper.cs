using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Application.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class ProductMapper
{
    public static ProductDto Map(this ProductEntity productEntity)
    {
        var result = new ProductDto()
        {
            ProductId = productEntity.ProductId,
            Name = productEntity.Name,
            Price = productEntity.Price,
            Quantity = productEntity.Quantity,
        };

        return result;
    }

    public static ProductEntity Map(this ProductDto productDto)
    {
        var result = new ProductEntity()
        {
            ProductId = productDto.ProductId,
            Name = productDto.Name,
            Price = productDto.Price,
            Quantity = productDto.Quantity,
        };

        return result;
    }
}
