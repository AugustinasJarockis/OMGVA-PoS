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
        public long? DiscountId { get; set; }

        // navigational properties
        public ICollection<OrderItem> OrderItems { get; set; }  // Order can have (order)items 
        public ICollection<Payment> Payments { get; set; } // order can have multiple / split payments
        // for foreign keys
        public Discount Discount { get; set; }
        public User User {  get; set; }
    }
}
