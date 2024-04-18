using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Requests;

[ExcludeFromCodeCoverage]
public class ProductRequest
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Preço é obrigatório.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Quantidade é obrigatório.")]
    public int Quantity { get; set; }
}
