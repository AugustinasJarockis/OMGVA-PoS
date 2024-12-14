using OmgvaPOS.Database.Context;
using OmgvaPOS.GiftcardManagement.Models;

namespace OmgvaPOS.GiftcardManagement.Repository
{
    public class GiftcardRepository(OmgvaDbContext database) : IGiftcardRepository
    {
        private readonly OmgvaDbContext _database = database;

        public Giftcard? GetGiftcardById(long id)
        {
            return _database.Giftcards.FirstOrDefault(g => g.Id == id);
        }
        public Giftcard? GetGiftcardByCode(string code)
        {
            return _database.Giftcards.FirstOrDefault(g => g.Code == code);
        }
        public Giftcard Create(Giftcard giftcard)
        {
            var createdGiftcard = _database.Giftcards.Add(giftcard).Entity;
            _database.SaveChanges();

            return createdGiftcard;
        }
        public void Update(string code, decimal calculatedBalance)
        {
            var giftcard = GetGiftcardByCode(code);

            if(giftcard != null)
            {
                giftcard.Balance = calculatedBalance;
                _database.SaveChanges();
            }
        }
        public List<Giftcard> GetGiftcards(long businessId)
        {
            return [.._database.Giftcards.Where(g => g.BusinessId == businessId)];
        }
    }
}
