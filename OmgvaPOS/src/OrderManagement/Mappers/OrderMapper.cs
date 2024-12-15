using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderItemManagement.Mappers;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.OrderManagement.Mappers;

public static class OrderMapper
{
    public static Order RequestToOrder(long businessId, long userId) {
        return new Order {
            Status = OrderStatus.Open,
            Tip = 0,
            BusinessId = businessId,
            UserId = userId
        };
    }

    public static OrderDTO OrderToDTO(this Order order) {
        return new OrderDTO {
            Id = order.Id,
            Status = order.Status,
            Tip = order.Tip,
            RefundReason = order.RefundReason,
            UserId = order.UserId,
            DiscountId = order.DiscountId,
            OrderItems = order.OrderItems?
                .Select(OrderItemMapper.OrderItemToDTO)
                .ToList() ?? []
        };
    }
}
