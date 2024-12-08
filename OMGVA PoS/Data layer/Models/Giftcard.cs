namespace OMGVA_PoS.Data_layer.Models
{
    public class Giftcard
    {
        public long Id { get; set; }
        public decimal Value { get; set; }
        public decimal Balance { get; set; }

        // navigation properties
        public ICollection<GiftcardPayment> GiftcardPayments { get; set; } // Giftcard can be paid with
    }
}
