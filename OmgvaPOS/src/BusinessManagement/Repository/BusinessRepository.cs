using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.Database.Context;

namespace OmgvaPOS.BusinessManagement.Repository
{
    public class BusinessRepository(OmgvaDbContext database) : IBusinessRepository
    {
        private readonly OmgvaDbContext _database = database;
        public List<Business> GetBusinesses() {
            return [.. _database.Businesses];
        }
        public Business? GetBusiness(long businessId) {
            return _database.Businesses.FirstOrDefault(b => b.Id == businessId)!;
        }
        public Business CreateBusiness(Business business) {
            _database.Businesses.Add(business);
            _database.SaveChanges();

            return business;
        }
        public bool UpdateBusiness(Business business)
        {
            _database.Update(business);
            _database.SaveChanges();
            return true;
        }
    }
}
