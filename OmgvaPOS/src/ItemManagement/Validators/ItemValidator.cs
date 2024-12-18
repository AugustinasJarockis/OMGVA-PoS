using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.Validators;

namespace OmgvaPOS.ItemManagement.Validators;

public static class ItemValidator
{
    public static void Exists(Item item) {
        if (item == null)
            throw new NotFoundException("Item doesn't exist/not found");
    }
    public static void IsNotArchived(Item item) {
        if (item.IsArchived)
            throw new ValidationException("Item is archived");
    }
    public static void EnoughInventoryQuantity(Item item, int amount) {
        ValidInventoryQuantity(item);
        if (item.InventoryQuantity < amount)
            throw new ValidationException($"Not enough {item.Name} left. Inventory has: {item.InventoryQuantity}, requested: {amount}.");
    }
    private static void ValidInventoryQuantity(Item item) {
        if (item.InventoryQuantity < 0)
            throw new ValidationException($"Fatal error. Inventory quantity of {item.Name} is less than 0.");
    }

    public static void ValidateCreateItemRequest(CreateItemRequest createItemRequest) {
        if (!createItemRequest.Currency.IsValidCurrency())
            throw new BadRequestException("Currency is not valid");

        if (createItemRequest.InventoryQuantity < 0)
            throw new BadRequestException("Inventory quantity can not be negative");

        if (createItemRequest.Price < 0)
            throw new BadRequestException("Price can not be negative");
    }

    public static void ValidateCreateItemRequest(UpdateItemRequest updateItemRequest) {
        if (!updateItemRequest.Currency.IsValidCurrency())
            throw new BadRequestException("Currency is not valid");

        if (updateItemRequest.InventoryQuantity < 0)
            throw new BadRequestException("Inventory quantity can not be negative");

        if (updateItemRequest.Price < 0)
            throw new BadRequestException("Price can not be negative");
    }
}
