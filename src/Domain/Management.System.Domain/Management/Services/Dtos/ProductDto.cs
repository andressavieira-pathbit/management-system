namespace Management.System.Domain.Management.Services.Dtos;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string AccessToken { get; set; } = default!;
}
