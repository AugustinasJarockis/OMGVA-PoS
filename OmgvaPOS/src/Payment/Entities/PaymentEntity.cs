using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OmgvaPOS.Customer.Entities;
using OmgvaPOS.GiftcardPayment.Entities;
using OmgvaPOS.Order.Entities;
using OmgvaPOS.Payment.Enums;

namespace OmgvaPOS.Payment.Entities
{
    public class PaymentEntity
    {
        [Key]
        public string Id { get; set; }
        public PaymentMethod Method { get; set; }
        public long CustomerId { get; set; }
        public long? GiftCardPaymentId { get; set; }
        public long OrderId { get; set; }
        
        // navigational properties
        // for foreign keys
        public GiftcardPaymentEntity GiftcardPaymentEntity { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
        
        [ForeignKey("OrderId")]
        [InverseProperty("PaymentEntity")]
        public OrderEntity OrderEntity { get; set; }

    }

}
