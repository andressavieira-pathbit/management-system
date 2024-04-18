using Management.System.Domain.Management.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Domain.Management.Entities;

[ExcludeFromCodeCoverage]
public class UserEntity
{
    [Key]
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public EUserType UserType { get; set; }
}
