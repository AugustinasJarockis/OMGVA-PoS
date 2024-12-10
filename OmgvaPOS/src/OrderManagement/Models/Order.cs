using System.ComponentModel.DataAnnotations.Schema;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderManagement.Enums;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.OrderManagement.Models
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

        // for foreign keys
        [InverseProperty("Order")]  // Add this annotation
        public Payment Payment { get; set; }    
        public Discount Discount { get; set; }
        public User User {  get; set; }
    }
}
