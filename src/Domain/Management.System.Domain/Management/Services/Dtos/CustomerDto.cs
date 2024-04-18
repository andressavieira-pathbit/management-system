namespace Management.System.Domain.Management.Services.Dtos;

public class CustomerDto
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}
