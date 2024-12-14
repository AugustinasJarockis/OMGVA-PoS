using OmgvaPOS.GiftcardManagement.Models;

namespace OmgvaPOS.GiftcardManagement.Repository
{
    public interface IGiftcardRepository
    {
        public Giftcard? GetGiftcardById(long id);
        public Giftcard? GetGiftcardByCode(string code);
        public Giftcard Create(Giftcard giftcard);
        public void Update(string code, decimal calculatedBalance);
        public List<Giftcard> GetGiftcards(long businessId);
    }
}
