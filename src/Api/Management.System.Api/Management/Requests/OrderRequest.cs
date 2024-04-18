using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Api.Management.Requests;

[ExcludeFromCodeCoverage]
public class OrderRequest
{
    [Required(ErrorMessage = "ProductId é obrigatório.")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantidade é obrigatório.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Cep é obrigatório.")]
    public string ZipCode { get; set; } = default!;

    [Required(ErrorMessage = "Número do endereço é obrigatório.")]
    public int NumberAddress { get; set; }
}
