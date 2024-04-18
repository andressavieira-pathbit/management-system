using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Application.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class CustomerMapper
{
    public static CustomerDto Map(this CustomerEntity customerEntity)
    {
        var result = new CustomerDto()
        {
            CustomerId = customerEntity.CustomerId,
            Email = customerEntity.Email,
            Name = customerEntity.Name,
            UserId = customerEntity.UserId
        };

        return result;
    }

    public static CustomerEntity Map(this CustomerDto customerDto)
    {
        var result = new CustomerEntity()
        {
            CustomerId = customerDto.CustomerId,
            Name = customerDto.Name,
            Email = customerDto.Email,
            UserId = customerDto.UserId
        };

        return result;
    }
}
