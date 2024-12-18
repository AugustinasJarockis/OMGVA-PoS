using OmgvaPOS.Exceptions;
using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Validators;

public static class OrderItemValidator
{
    public static void OrderItemNotInOrderYet(Order order, OrderItem newOrderItem) {
        foreach (var orderItem in order.OrderItems) {
            // if same ItemId
            if (orderItem.ItemId == newOrderItem.ItemId) {
                // and if same variations
                if (orderItem.OrderItemVariations == newOrderItem.OrderItemVariations) {
                    throw new BadRequestException("This exact item is already in the order");
                }
            }
        }
    }

    public static void Exists(OrderItem orderItem) {
        if (orderItem == null) {
            throw new NotFoundException("Order Item not found.");
        }
    }

    public static void ValidateCreateOrderItemRequest(CreateOrderItemRequest createRequest)
    {
        if (createRequest.Quantity < 0)
            throw new BadRequestException("Quantity cannot be negative.");
    }
    
    public static void ValidateUpdateOrderItemRequest(UpdateOrderItemRequest updateRequest)
    {
        if (updateRequest.Quantity < 0)
            throw new BadRequestException("Quantity cannot be negative.");
    }
    
}
