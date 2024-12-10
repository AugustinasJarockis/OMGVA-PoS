using OmgvaPOS.GiftcardManagement.Models;

namespace OmgvaPOS.GiftcardPaymentManagement.Models
{
    public class GiftcardPaymentEntity
    {
        public long Id { get; set; }
        public long GiftcardId { get; set; }
        public decimal AmountUsed { get; set; }

        // navigation properties
        // for foreign keys
        public Giftcard Giftcard { get; set; }
    }
}
