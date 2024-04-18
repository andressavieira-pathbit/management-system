using Management.System.Domain.Management.Enums;

namespace Management.System.Domain.Management.Services.Dtos;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public EUserType UserType { get; set; }
}
