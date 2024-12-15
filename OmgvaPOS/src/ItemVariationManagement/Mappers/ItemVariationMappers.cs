using OmgvaPOS.ItemVariationManagement.DTOs;
using OmgvaPOS.ItemVariationManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OmgvaPOS.ItemVariationManagement.Mappers
{
    public static class ItemVariationMappers
    {
        public static ItemVariation ToItemVariation(this ItemVariationCreationRequest request) {
            return new ItemVariation() {
                Name = request.Name,
                InventoryQuantity = request.InventoryQuantity,
                PriceChange = request.PriceChange,
                ItemVariationGroup = request.ItemVariationGroup,
                IsArchived = false
            };
        }

        public static ItemVariation ToItemVariation(this ItemVariationUpdateRequest request, ItemVariation baseVariation) {
            baseVariation.Id = baseVariation.Id;
            baseVariation.ItemId = baseVariation.ItemId;
            baseVariation.Name = request.Name ?? baseVariation.Name;
            baseVariation.InventoryQuantity = request.InventoryQuantity ?? baseVariation.InventoryQuantity;
            baseVariation.PriceChange = request.PriceChange ?? baseVariation.PriceChange;
            baseVariation.ItemVariationGroup = request.ItemVariationGroup ?? baseVariation.ItemVariationGroup;
            baseVariation.IsArchived = false;
            return baseVariation;
        }

        public static ItemVariationDTO ToItemVariationDTO(this ItemVariation variation) {
            return new ItemVariationDTO() {
                Id = variation.Id,
                ItemId = variation.ItemId,
                Name = variation.Name,
                InventoryQuantity = variation.InventoryQuantity,
                PriceChange = variation.PriceChange,
                ItemVariationGroup = variation.ItemVariationGroup
            };
        }
    }
}
