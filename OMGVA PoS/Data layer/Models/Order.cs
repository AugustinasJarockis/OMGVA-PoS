using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class Order
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Tip { get; set; }
        public string RefundReason { get; set; }
        public long UserId { get; set; }
        public long PaymentId { get; set; }
        public long? DiscountId { get; set; }
    }
}
