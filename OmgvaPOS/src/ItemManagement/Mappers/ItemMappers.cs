using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;

namespace OmgvaPOS.src.ItemManagement.Mappers
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

        public static Item ToItem(this ItemDTO itemDTO, Item baseItem) {
            return new() {
                Id = baseItem.Id,
                Name = itemDTO.Name ?? baseItem.Name,
                InventoryQuantity = itemDTO.InventoryQuantity ?? baseItem.InventoryQuantity,
                Price = itemDTO.Price ?? baseItem.Price,
                Currency = itemDTO.Currency ?? baseItem.Currency,
                ItemGroup = itemDTO.ItemGroup ?? baseItem.ItemGroup,
                Duration = itemDTO.Duration ?? baseItem.Duration,
                ImgPath = itemDTO.ImgPath ?? baseItem.ImgPath,
                DiscountId = itemDTO.DiscountId ?? baseItem.DiscountId,
                UserId = itemDTO.UserId ?? baseItem.UserId
            };
        }
    }
}
