namespace OMGVA_PoS.Data_layer.Models
{
    public class GiftcardPayment
    {
        public long Id { get; set; }
        public long GiftcardId { get; set; }
        public decimal AmountUsed { get; set; }

        // navigation properties
        // for foreign keys
        public Giftcard Giftcard { get; set; }
    }
}
