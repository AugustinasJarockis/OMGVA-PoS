namespace OMGVA_PoS.Data_layer.Models
{
    public class GiftCardPayment
    {
        public long Id { get; set; }
        public long GiftCardId { get; set; }
        public decimal AmountUsed { get; set; }

        // navigation properties
        // for foreign keys
        public GiftCard GiftCard { get; set; }
    }
}
