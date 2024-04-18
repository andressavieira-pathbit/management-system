using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Management.System.Api.Management.Requests;

[ExcludeFromCodeCoverage]
public class CustomerRequest
{
    [JsonIgnore]
    public Guid CustomerId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = default!;
}
