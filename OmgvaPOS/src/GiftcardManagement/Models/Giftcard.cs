using OmgvaPOS.GiftcardPaymentManagement.Models;

namespace OmgvaPOS.GiftcardManagement.Models
{
    public class Giftcard
    {
        public long Id { get; set; }
        public decimal Value { get; set; }
        public decimal Balance { get; set; }

        // navigation properties
        public ICollection<GiftcardPaymentEntity> GiftcardPayments { get; set; } // Giftcard can be paid with
    }
}
