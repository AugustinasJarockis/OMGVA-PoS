using Microsoft.OpenApi.Any;
using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemVariationManagement.Models;

namespace OmgvaPOS.ItemVariationManagement.Validators;

public static class ItemVariationValidator
{
    public static void AnyVariationsExist(IEnumerable<ItemVariation> itemVariations) {
        if (!itemVariations.Any()) {
            throw new NotFoundException($"No variations found for item.");
        }
    }

    public static void Exists(ItemVariation itemVariation) {
        if (itemVariation == null) {
            throw new NotFoundException("Item variation not found");
        }
    }

    public static void IsNotArchived(ItemVariation itemVariation) {
        if (itemVariation.IsArchived) {
            throw new ValidationException($"Item Variation {itemVariation.Name} is archived");
        }
    }

    public static void EnoughInventoryQuantity(ItemVariation itemVariation, int amount) {
        if (itemVariation.InventoryQuantity < amount)
            throw new ValidationException($"Not enough {itemVariation.Name} left. Inventory has: {itemVariation.InventoryQuantity}, requested: {amount}.");
    }
}
