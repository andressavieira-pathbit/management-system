using Management.System.Domain.Management.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Management.System.Api.Management.Requests;

[ExcludeFromCodeCoverage]
public class UserRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Nome de usuário é obrigatório.")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [RegularExpression(@"^^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[-_@&]).{8,}$",
     ErrorMessage = "A senha deve conter pelo menos 8 caracteres, incluindo uma letra maiúscula, uma letra minúscula, um número e um caractere especial.")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Tipo de usuário é obrigatório.")]
    public EUserType UserType { get; set; }
}
