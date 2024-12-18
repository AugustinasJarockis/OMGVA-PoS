using OmgvaPOS.GiftcardPaymentManagement.Models;

namespace OmgvaPOS.GiftcardManagement.Models
{
    public class Giftcard
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public decimal Balance { get; set; }
    }
}
