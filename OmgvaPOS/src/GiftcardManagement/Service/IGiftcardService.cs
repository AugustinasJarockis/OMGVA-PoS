using OmgvaPOS.GiftcardManagement.DTOs;
using OmgvaPOS.GiftcardManagement.Models;

namespace OmgvaPOS.GiftcardManagement.Service
{
    public interface IGiftcardService
    {
        public Giftcard? GetGiftcard(long id);
        public Giftcard? GetGiftcard(string code);
        public Giftcard CreateGiftcard(GiftcardDTO giftcard);
        public void UpdateGiftcard(GiftcardUpdateRequest giftcardUpdateRequest);
        public List<Giftcard> GetGiftcards(long businessId);
    }
}
