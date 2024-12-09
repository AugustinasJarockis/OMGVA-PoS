using OmgvaPOS.Giftcard.Entities;

namespace OmgvaPOS.GiftcardPayment.Entities
{
    public class GiftcardPaymentEntity
    {
        public long Id { get; set; }
        public long GiftcardId { get; set; }
        public decimal AmountUsed { get; set; }

        // navigation properties
        // for foreign keys
        public GiftcardEntity GiftcardEntity { get; set; }
    }
}
