using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Domain.Management.Entities;

[ExcludeFromCodeCoverage]
public class ProductEntity
{
    [Key]
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
