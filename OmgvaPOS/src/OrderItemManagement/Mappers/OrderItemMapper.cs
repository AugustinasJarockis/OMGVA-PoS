using OmgvaPOS.OrderItemManagement.DTOs;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemVariationManagement.Mappers;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;

namespace OmgvaPOS.OrderItemManagement.Mappers;

public static class OrderItemMapper
{
    public static OrderItem RequestToOrderItem(this CreateOrderItemRequest request) {
        return new OrderItem {
            ItemId = request.ItemId,
            Quantity = request.Quantity,
            OrderItemVariations = request.ItemVariationIds?
                .Select(itemVariationId => new OrderItemVariation {
                    ItemVariationId = itemVariationId
                })
                .ToList()
        };
    }
    public static OrderItemDTO OrderItemToDTO(OrderItem newOrderItem) {
        return new OrderItemDTO {
            Id = newOrderItem.Id,
            ItemId = newOrderItem.ItemId,
            Quantity = newOrderItem.Quantity,
            DiscountId = newOrderItem.DiscountId,
            OrderItemVariations = newOrderItem.OrderItemVariations?
                .Select(OrderItemVariationMapper.OrderItemVariationToDTO)
                .ToList()
        };
    }
}
