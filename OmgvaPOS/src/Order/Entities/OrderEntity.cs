using OmgvaPOS.UserManagement.Entities;

namespace OmgvaPOS.Order.Entities
{
    public class OrderEntity
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Tip { get; set; }
        public string RefundReason { get; set; }
        public string PaymentId { get; set; }
        public long UserId { get; set; }
        public long? DiscountId { get; set; }

        // navigational properties
        public ICollection<OrderItem.Entities.OrderItemEntity> OrderItems { get; set; }  // Order can have (order)items 

        // for foreign keys
        public Payment.Entities.PaymentEntity PaymentEntity { get; set; }    
        public Discount.Entities.DiscountEntity DiscountEntity { get; set; }
        public UserEntity UserEntity {  get; set; }
    }
}
