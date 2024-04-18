using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Application.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class UserMapper
{
    public static UserDto Map(this UserEntity userEntity)
    {
        var result = new UserDto()
        {
            UserId = userEntity.UserId,
            Email = userEntity.Email,
            UserName = userEntity.UserName,
            Password = userEntity.Password,
            UserType = userEntity.UserType,
        };

        return result;
    }

    public static UserEntity Map(this UserDto userDto)
    {
        var result = new UserEntity()
        {
            UserId = userDto.UserId,
            Email = userDto.Email,
            UserName = userDto.UserName,
            Password = userDto.Password,
            UserType = userDto.UserType,
        };

        return result;
    }
}
