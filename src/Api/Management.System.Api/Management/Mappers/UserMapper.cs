using Management.System.Api.Management.Requests;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class UserMapper
{
    public static UserDto Map(this UserRequest userRequest)
    {
        var result = new UserDto()
        {
            Email = userRequest.Email,
            UserName = userRequest.UserName,
            Password = userRequest.Password,
            UserType = userRequest.UserType,
        };

        return result;
    }
}
