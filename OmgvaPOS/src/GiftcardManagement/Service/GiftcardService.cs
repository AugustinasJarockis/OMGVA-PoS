using OmgvaPOS.Exceptions;
using OmgvaPOS.GiftcardManagement.DTOs;
using OmgvaPOS.GiftcardManagement.Mappers;
using OmgvaPOS.GiftcardManagement.Models;
using OmgvaPOS.GiftcardManagement.Repository;

namespace OmgvaPOS.GiftcardManagement.Service
{
    public class GiftcardService(IGiftcardRepository giftcardRepository) : IGiftcardService
    {
        private readonly IGiftcardRepository _giftcardRepository = giftcardRepository;
        private static readonly Random random = new();

        public Giftcard? GetGiftcard(long id)
        {
            return _giftcardRepository.GetGiftcardById(id);
        }
        public Giftcard? GetGiftcard(string code)
        {
            return _giftcardRepository.GetGiftcardByCode(code);
        }
        public Giftcard CreateGiftcard(GiftcardDTO giftcard)
        {
            if (giftcard.Value <= 0)
                throw new ApplicationException("Value should be positive");

            string code;
            do
            {
                code = RandomString();
            } while (_giftcardRepository.GetGiftcardByCode(code) != null);

            return _giftcardRepository.Create(GiftcardMapper.FromCreate(giftcard, code));
        }
        public void UpdateGiftcard(GiftcardUpdateRequest giftcardUpdateRequest)
        {
            var giftcard = _giftcardRepository.GetGiftcardByCode(giftcardUpdateRequest.Code);
            if (giftcard == null)
                return;

            if (giftcard.Balance < giftcardUpdateRequest.Amount)
                throw new BadRequestException("Giftcard does not have enough balance");
                
            if (giftcardUpdateRequest.Amount <= 0)
                throw new BadRequestException("Amount must be positive");

            decimal calculatedAmount = giftcard.Balance - giftcardUpdateRequest.Amount;
            _giftcardRepository.Update(giftcardUpdateRequest.Code, calculatedAmount);
        }
        public List<Giftcard> GetGiftcards(long businessId)
        {
            return _giftcardRepository.GetGiftcards(businessId);
        }
        private static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
