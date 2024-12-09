namespace OmgvaPOS.GiftcardPayment.Entities
{
    public class GiftcardPaymentEntity
    {
        public long Id { get; set; }
        public long GiftcardId { get; set; }
        public decimal AmountUsed { get; set; }

        // navigation properties
        // for foreign keys
        public Giftcard.Entities.GiftcardEntity GiftcardEntity { get; set; }
    }
}
