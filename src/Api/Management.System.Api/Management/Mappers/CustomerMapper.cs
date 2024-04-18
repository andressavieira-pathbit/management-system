using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class CustomerMapper
{
    public static CustomerDto Map(this CustomerRequest customerRequest)
    {
        var result = new CustomerDto()
        {
            CustomerId = customerRequest.CustomerId,
            Name = customerRequest.Name,
        };

        return result;
    }
}
