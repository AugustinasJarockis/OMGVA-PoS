using OmgvaPOS.Payment.Enums;

namespace OmgvaPOS.Payment.Entities
{
    public class PaymentEntity
    {
        public string Id { get; set; }
        public PaymentMethod Method { get; set; }
        public long CustomerId { get; set; }
        public long? GiftCardPaymentId { get; set; }
        
        // navigational properties
        // for foreign keys
        public GiftcardPayment.Entities.GiftcardPaymentEntity GiftcardPaymentEntity { get; set; }
        public Customer.Entities.CustomerEntity CustomerEntity { get; set; }
        public Order.Entities.OrderEntity OrderEntity { get; set; }

    }

}
