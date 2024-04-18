using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class ProductMapper
{
    public static ProductDto Map(this ProductRequest productRequest, string acessToken)
    {
        var result = new ProductDto()
        {
            Name = productRequest.Name,
            Price = productRequest.Price,
            Quantity = productRequest.Quantity,
            AccessToken = acessToken
        };

        return result;
    }
}
