using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OmgvaPOS.CustomerManagement.Models;
using OmgvaPOS.GiftcardPaymentManagement.Models;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.PaymentManagement.Enums;

namespace OmgvaPOS.PaymentManagement.Models
{
    public class Payment
    {
        [Key]
        public string Id { get; set; }
        public PaymentMethod Method { get; set; }
        public long CustomerId { get; set; }
        public long? GiftCardPaymentId { get; set; }
        public long OrderId { get; set; }
        public long Amount { get; set; }
        
        // navigational properties
        // for foreign keys
        public GiftcardPaymentEntity? GiftcardPaymentEntity { get; set; }
        public Customer Customer { get; set; }
        
        [ForeignKey("OrderId")]
        [InverseProperty("Payment")]
        public Order Order { get; set; }

    }

}
