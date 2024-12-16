using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderManagement.DTOs;

namespace OmgvaPOS.OrderItemVariationManagement.Mappers;

public static class OrderItemVariationMapper
{

    public static OrderItemVariationDTO OrderItemVariationToDTO(OrderItemVariation variation) {
        return new OrderItemVariationDTO {
            Id = variation.Id,
            ItemVariationId = variation.ItemVariationId
        };
    }
}
