using OmgvaPOS.GiftcardManagement.DTOs;
using OmgvaPOS.GiftcardManagement.Models;

namespace OmgvaPOS.GiftcardManagement.Mappers
{
    public class GiftcardMapper
    {
        public static Giftcard FromCreate(GiftcardDTO giftcard, string code)
        {
            return new Giftcard()
            {
                BusinessId = giftcard.BusinessId,
                Code = code,
                Value = giftcard.Value,
                Balance = giftcard.Value
            };
        }
    }
}
