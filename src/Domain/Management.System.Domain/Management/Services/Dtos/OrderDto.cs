using Management.System.Domain.Management.Enums;

namespace Management.System.Domain.Management.Services.Dtos;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public EOrderStatus Status { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ZipCode { get; set; } = default!;
    public string DeliveryAddress { get; set; } = default!;
    public int NumberAddress { get; set; }
    public Guid CustomerId { get; set; } = default!;
    public Guid ProductId { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
}
