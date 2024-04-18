using Management.System.Domain.Management.Entities;
using Management.System.Domain.Management.Enums;
using Management.System.Domain.Management.Services.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Application.Management.Mappers;

[ExcludeFromCodeCoverage]
public static class OrderMapper
{
    public static OrderDto Map(this OrderEntity orderEntity)
    {
        var result = new OrderDto()
        {
            OrderId = orderEntity.OrderId,
            OrderDate = orderEntity.OrderDate,
            Status = orderEntity.Status,
            Quantity = orderEntity.Quantity,
            Price = orderEntity.Price,
            ZipCode = orderEntity.ZipCode,
            DeliveryAddress = orderEntity.DeliveryAddress,
            NumberAddress = orderEntity.NumberAddress,
            CustomerId = orderEntity.CustomerId,
            ProductId = orderEntity.ProductId
        };

        return result;
    }

    public static OrderEntity Map(this OrderDto orderDto)
    {
        var result = new OrderEntity()
        {
            OrderId = orderDto.OrderId,
            OrderDate = DateTime.Now,
            Status = EOrderStatus.Shipped,
            Quantity = orderDto.Quantity,
            Price = orderDto.Price,
            ZipCode = orderDto.ZipCode,
            DeliveryAddress = orderDto.DeliveryAddress,
            NumberAddress = orderDto.NumberAddress,
            CustomerId = orderDto.CustomerId,
            ProductId = orderDto.ProductId
        };

        return result;
    }
}
