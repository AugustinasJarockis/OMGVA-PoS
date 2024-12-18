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

    public static void Exists(ItemVariation? itemVariation) {
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
    
    public static void ValidateNoItemVariationsFromSameGroup(List<ItemVariation>? itemVariations) {
        if (itemVariations == null || itemVariations.Count < 2)
            return;
        
        var variationGroups = itemVariations
            .Select(oiv => oiv.ItemVariationGroup)
            .GroupBy(group => group)
            .ToList();

        // If any group appears more than once, throw an exception
        var duplicateGroups = variationGroups
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateGroups.Count != 0)
        {
            throw new ValidationException(
                $"Multiple variations from the same group(s) are not allowed: {string.Join(", ", duplicateGroups)}"
            );
        }
    }

    public static void ValidateItemVariationBelongsToItem(ItemVariation itemVariation, Item item)
    {
        if (itemVariation.ItemId != item.Id)
            throw new ValidationException($"Variation {itemVariation.Name} does not belong to item {item.Name}");
    }
    
}
