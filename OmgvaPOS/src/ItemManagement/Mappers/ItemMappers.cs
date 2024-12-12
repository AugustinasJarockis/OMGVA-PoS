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

        public static Item ToItem(this ItemDTO itemDTO) {
            return new() {
                Id = itemDTO.Id,
                Name = itemDTO.Name,
                InventoryQuantity = itemDTO.InventoryQuantity,
                Price = itemDTO.Price,
                Currency = itemDTO.Currency,
                ItemGroup = itemDTO.ItemGroup,
                Duration = itemDTO.Duration,
                ImgPath = itemDTO.ImgPath,
                DiscountId = itemDTO.DiscountId,
                UserId = itemDTO.UserId
            };
        }
    }
}
