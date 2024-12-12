using OmgvaPOS.ItemVariationManagement.DTOs;
using OmgvaPOS.ItemVariationManagement.Models;

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

        public static ItemVariation ToItemVariation(this ItemVariationDTO variation, ItemVariation baseVariation) {
            baseVariation.Id = baseVariation.Id;
            baseVariation.ItemId = variation.ItemId ?? baseVariation.ItemId;
            baseVariation.Name = variation.Name ?? baseVariation.Name;
            baseVariation.InventoryQuantity = variation.InventoryQuantity ?? baseVariation.InventoryQuantity;
            baseVariation.PriceChange = variation.PriceChange ?? baseVariation.PriceChange;
            baseVariation.ItemVariationGroup = variation.ItemVariationGroup ?? baseVariation.ItemVariationGroup;
            baseVariation.IsArchived = false;
            return baseVariation;
        }
    }
}
