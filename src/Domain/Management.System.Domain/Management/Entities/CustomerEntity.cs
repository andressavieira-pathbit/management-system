using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Domain.Management.Entities;

[ExcludeFromCodeCoverage]
public class CustomerEntity
{
    [Key]
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;

    [ForeignKey("UserId")]
    public Guid UserId { get; set; } = default!;
}
