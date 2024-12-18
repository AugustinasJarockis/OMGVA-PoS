using OmgvaPOS.OrderManagement.Enums;

namespace OmgvaPOS.OrderManagement.DTOs
{
    public class UpdateOrderRequest
    {
        public OrderStatus? Status { get; set; }
        public decimal? Tip { get; set; }
        public string? RefundReason { get; set; }
        public long? UserId { get; set; }
    }
}