using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public PaymentMethod Method { get; set; }
        public long CustomerId { get; set; }
        public long? GiftcardPaymentId { get; set; }
        
        // navigational properties
        // for foreign keys
        public GiftcardPayment GiftcardPayment { get; set; }
        public Customer Customer { get; set; }
        public Order Order { get; set; }

    }

}
