using OmgvaPOS.Exceptions;
using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Validators;

public static class OrderItemValidator
{

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
