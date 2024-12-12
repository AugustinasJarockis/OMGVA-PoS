using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;

namespace OmgvaPOS.src.ItemManagement.Mappers
{
    public static class ItemMappers
    {
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
