using OmgvaPOS.Exceptions;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.Validators;

namespace OmgvaPOS.ItemManagement.Validator;

public static class ItemValidator
{
    public static void ValidateCreateItemRequest(CreateItemRequest createItemRequest)
    {
        if (!createItemRequest.Currency.IsValidCurrency())
            throw new BadRequestException("Currency is not valid");

        if (createItemRequest.InventoryQuantity < 0)
            throw new BadRequestException("Inventory quantity can not be negative");

        if (createItemRequest.Price < 0)
            throw new BadRequestException("Price can not be negative");
    }
    
    public static void ValidateCreateItemRequest(UpdateItemRequest updateItemRequest)
    {
        if (!updateItemRequest.Currency.IsValidCurrency())
            throw new BadRequestException("Currency is not valid");

        if (updateItemRequest.InventoryQuantity < 0)
            throw new BadRequestException("Inventory quantity can not be negative");

        if (updateItemRequest.Price < 0)
            throw new BadRequestException("Price can not be negative");
    }
    

}