using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;

namespace OmgvaPOS.ItemManagement.Mappers
{
    public static class ItemMappers
    {
        public static Item ToItem(this CreateItemRequest request, long businessId) {
            return new Item {
                Name = request.Name,
                InventoryQuantity = request.InventoryQuantity,
                Price = request.Price,
                Currency = request.Currency,
                ItemGroup = request.ItemGroup,
                Duration = request.Duration,
                ImgPath = request.ImgPath,
                IsArchived = false,
                BusinessId = businessId,
                DiscountId = request.DiscountId,
                UserId = request.UserId
            };
        }
        public static ItemDTO ToItemDTO(this Item item) {
            return new() {
                Id = item.Id,
                Name = item.Name,
                InventoryQuantity = item.InventoryQuantity,
                Price = item.Price,
                Currency = item.Currency,
                ItemGroup = item.ItemGroup,
                Duration = item.Duration,
                ImgPath = item.ImgPath,
                DiscountId = item.DiscountId,
                UserId = item.UserId
            };
        }

        public static Item FromUpdateRequestToItem(this ItemDTO updateItemDTO, Item baseItem) {
            baseItem.Id = baseItem.Id;
            baseItem.Name = updateItemDTO.Name ?? baseItem.Name;
            baseItem.InventoryQuantity = updateItemDTO.InventoryQuantity ?? baseItem.InventoryQuantity;
            baseItem.Price = updateItemDTO.Price ?? baseItem.Price;
            baseItem.Currency = updateItemDTO.Currency ?? baseItem.Currency;
            baseItem.ItemGroup = updateItemDTO.ItemGroup ?? baseItem.ItemGroup;
            baseItem.Duration = updateItemDTO.Duration ?? baseItem.Duration;
            baseItem.ImgPath = updateItemDTO.ImgPath ?? baseItem.ImgPath;
            baseItem.DiscountId = updateItemDTO.DiscountId ?? baseItem.DiscountId;
            baseItem.UserId = updateItemDTO.UserId ?? baseItem.UserId;
            return baseItem;
        }
    }
}
