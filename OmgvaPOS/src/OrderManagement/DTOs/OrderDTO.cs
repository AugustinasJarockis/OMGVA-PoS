using OmgvaPOS.OrderManagement.Enums;

namespace OmgvaPOS.OrderManagement.DTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Tip { get; set; }
        public string? RefundReason { get; set; }
        public long UserId { get; set; }
        public long? DiscountId { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
    public class OrderItemDTO
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public short Quantity { get; set; }
        public long? DiscountId { get; set; }
        public ICollection<OrderItemVariationDTO>? OrderItemVariations { get; set; }
    }

    public class OrderItemVariationDTO
    {
        public long Id { get; set; }
        public long ItemVariationId { get; set; }
    }
}
