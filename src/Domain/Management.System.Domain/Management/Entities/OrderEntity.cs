using Management.System.Domain.Management.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Domain.Management.Entities;

[ExcludeFromCodeCoverage]
public class OrderEntity
{
    [Key]
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public EOrderStatus Status { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ZipCode { get; set; } = default!;
    public string DeliveryAddress { get; set; } = default!;
    public int NumberAddress { get; set; }

    [ForeignKey("CustomerId")]
    public Guid CustomerId { get; set; } = default!;

    [ForeignKey("ProductId")]
    public Guid ProductId { get; set; } = default!;
}
