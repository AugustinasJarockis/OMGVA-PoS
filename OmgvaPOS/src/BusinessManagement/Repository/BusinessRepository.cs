using OmgvaPOS.AuthManagement.Repository;
using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Mappers;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.Database.Context;

namespace OmgvaPOS.BusinessManagement.Repository
{
    public class BusinessRepository(OmgvaDbContext database, IAuthenticationRepository authenticationRepository) : IBusinessRepository
    {
        private readonly OmgvaDbContext _database = database;
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        public List<Business> GetBusinesses() {
            return [.. _database.Businesses];
        }
        public Business GetBusiness(long businessId) {
            return _database.Businesses.Where(b => b.Id == businessId).FirstOrDefault()!;
        }
        public Business CreateBusiness(Business business) {
            _database.Businesses.Add(business);
            _database.SaveChanges();

            return business;
        }
        public bool UpdateBusiness(BusinessDTO business) {
            var businessToUpdate = _database.Businesses.SingleOrDefault(b => b.Id == business.Id);
            if (businessToUpdate != null) {
                businessToUpdate = business.ToBusiness(businessToUpdate);
                _database.Update(businessToUpdate);
                _database.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }
    }
}
