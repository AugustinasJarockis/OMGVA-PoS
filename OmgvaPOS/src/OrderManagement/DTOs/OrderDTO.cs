using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.OrderItemManagement.DTOs;

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
}
