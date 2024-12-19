using OmgvaPOS.Exceptions;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderManagement.Validators;

public static class OrderValidator
{
    public static void Exists(Order order) {
        if (order == null)
            throw new NotFoundException("Order doesn't exist/not found");
    }

    public static void Exist(IEnumerable<object> orders) {
        if (orders == null || !orders.Any())
            throw new NotFoundException("No Orders for business found");
    }

    public static void IsOpen(Order order) {
        if (order.Status != OrderStatus.Open)
            throw new ValidationException($"Order with ID {order.Id} is {order.Status} (not open for editing)");
    }

    public static void ValidateUpdateOrderRequest(UpdateOrderRequest updateRequest)
    {
        if (updateRequest.Tip != null & updateRequest.Tip < 0)
            throw new BadRequestException("Tip cannot be negative");
    }

    public static void CorrectSplitRequest(OrderItem originalOrderItem, SplitOrderItem splitOrderItem) {
        if (splitOrderItem.Quantity == 0)
            throw new BadRequestException("You have to split more than 0 of that item, cmon now.");
        if (originalOrderItem.Quantity < splitOrderItem.Quantity)
            throw new BadRequestException("Cannot split a higher quantity of an item than there already is in the order.");
    }
}
