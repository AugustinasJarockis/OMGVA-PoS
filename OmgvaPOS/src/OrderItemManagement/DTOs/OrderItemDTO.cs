using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderItemVariationManagement.DTOs;

namespace OmgvaPOS.OrderItemManagement.DTOs
{
    public class OrderItemDTO
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public short Quantity { get; set; }
        public long? DiscountId { get; set; }
        public OrderItemVariationDTO? OrderItemVariation { get; set; }
    }
}
