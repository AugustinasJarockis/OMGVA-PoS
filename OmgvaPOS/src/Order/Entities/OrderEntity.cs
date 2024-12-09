using System.ComponentModel.DataAnnotations.Schema;
using OmgvaPOS.Discount.Entities;
using OmgvaPOS.OrderItem.Entities;
using OmgvaPOS.Payment.Entities;
using OmgvaPOS.UserManagement.Entities;

namespace OmgvaPOS.Order.Entities
{
    public class OrderEntity
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Tip { get; set; }
        public string RefundReason { get; set; }
        public long UserId { get; set; }
        public long? DiscountId { get; set; }

        // navigational properties
        public ICollection<OrderItemEntity> OrderItems { get; set; }  // Order can have (order)items 

        // for foreign keys
        [InverseProperty("OrderEntity")]  // Add this annotation
        public PaymentEntity PaymentEntity { get; set; }    
        public DiscountEntity DiscountEntity { get; set; }
        public UserEntity UserEntity {  get; set; }
    }
}
