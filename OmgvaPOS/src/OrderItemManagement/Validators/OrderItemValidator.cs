using Azure.Core;
using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Enums;
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
}
