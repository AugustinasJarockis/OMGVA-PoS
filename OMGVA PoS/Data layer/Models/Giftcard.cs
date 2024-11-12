namespace OMGVA_PoS.Data_layer.Models
{
    public class GiftCard
    {
        public long Id { get; set; }
        public decimal Value { get; set; }
        public decimal Balance { get; set; }

        // navigation properties
        public ICollection<GiftCardPayment> GiftCardPayments { get; set; } // GiftCard can be paid with
    }
}
