using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.UserManagement.Models;
using OmgvaPOS.DiscountManagement.Models;

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


    public static SimpleUserDTO ToSimpleUserDTO(this User user) {
        if (user == null) return null;
        return new SimpleUserDTO {
            UserId = user.Id,
            UserName = user.Name
        };
    }

    public static SimpleDiscountDTO ToSimpleDiscountDTO(this Discount discount) {
        if (discount == null) return null;
        return new SimpleDiscountDTO {
            DiscountId = discount.Id,
            DiscountAmount = discount.Amount
        };
    }

    public static SimpleOrderDTO ToSimpleOrderDTO(this Order order) {
        if (order == null) return null;

        return new SimpleOrderDTO {
            Id = order.Id,
            Status = order.Status,
            Tip = order.Tip,
            RefundReason = order.RefundReason,
            User = order.User.ToSimpleUserDTO()
        };
    }

    public static IEnumerable<SimpleOrderDTO> ToSimpleOrderDTOList(this IEnumerable<Order> orders) {
        return orders?.Select(ToSimpleOrderDTO).ToList();
    }

    public static Order ToUpdatedOrder(this UpdateOrderRequest updateRequest, Order currentOrder)
    {
        currentOrder.Tip = updateRequest.Tip ?? currentOrder.Tip;
        currentOrder.UserId = updateRequest.UserId ?? currentOrder.UserId;
        currentOrder.RefundReason = updateRequest.RefundReason ?? currentOrder.RefundReason;

        return currentOrder;
    }
}
