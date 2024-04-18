using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Requests;

[ExcludeFromCodeCoverage]
public class LoginRequest
{
    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$",
    ErrorMessage = "A senha deve conter pelo menos 8 caracteres, incluindo uma letra maiúscula, uma letra minúscula, um número e um caractere especial.")]
    public string Password { get; set; } = default!;
}
