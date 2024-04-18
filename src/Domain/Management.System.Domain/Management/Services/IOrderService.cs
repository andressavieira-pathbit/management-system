using Management.System.Domain.Management.Services.Dtos;

namespace Management.System.Domain.Management.Services;

public interface IOrderService
{
    Task CreateAsync(OrderDto orderDto, CustomerDto customerDto);
}
