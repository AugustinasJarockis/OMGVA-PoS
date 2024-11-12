using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public PaymentMethod Method { get; set; }
        public long CustomerId { get; set; }
        public long? GiftCardPaymentId { get; set; }
        public long OrderId { get; set; }
        
        // navigational properties
        // for foreign keys
        public GiftCardPayment GiftCardPayment { get; set; }
        public Customer Customer { get; set; }
        public Order Order { get; set; }

    }

}
