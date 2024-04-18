using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class OrderMapper
{
    public static OrderDto Map(this OrderRequest orderRequest, string accessToken)
    {
        var result = new OrderDto
        {
            Quantity = orderRequest.Quantity,
            ZipCode = orderRequest.ZipCode,
            NumberAddress = orderRequest.NumberAddress,
            ProductId = orderRequest.ProductId,
            AccessToken = accessToken,
        };

        return result;
    }
}
