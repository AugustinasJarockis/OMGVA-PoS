using OmgvaPOS.Auth.Repository;
using OmgvaPOS.BusinessManagement.Entities;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.Database.Context;

namespace OmgvaPOS.BusinessManagement.Repository
{
    public class BusinessRepository(OmgvaDbContext database, IAuthenticationRepository authenticationRepository) : IBusinessRepository
    {
        private readonly OmgvaDbContext _database = database;
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        public BusinessDTO CreateBusiness(CreateBusinessRequest createBusinessRequest) {
            BusinessEntity business = new() {
                StripeAccId = "/////////////////////////",//TODO: somehow acquire stripe acc id
                Name = createBusinessRequest.Name,
                Address = createBusinessRequest.Address,
                Phone = createBusinessRequest.Phone,
                Email = createBusinessRequest.Email
            };

            _database.Businesses.Add(business);
            _database.SaveChanges();

            return new () {
                Id = business.Id,
                Name = business.Name,
                Address = business.Address,
                Phone = business.Phone,
                Email = business.Email
            };
        }

        public bool UpdateBusiness(long businessId, BusinessDTO business) {
            var businessToUpdate = _database.Businesses.SingleOrDefault(b => b.Id == businessId);
            if (businessToUpdate != null) {
                businessToUpdate.Name = business.Name ?? businessToUpdate.Name;
                businessToUpdate.Address = business.Address ?? businessToUpdate.Address;
                businessToUpdate.Phone = business.Phone ?? businessToUpdate.Phone;
                businessToUpdate.Email = business.Email ?? businessToUpdate.Email;
                _database.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }

        public List<BusinessDTO> GetBusinesses() {
            return [.. _database.Businesses.Select<BusinessEntity, BusinessDTO>(
                b => new() {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Phone = b.Phone,
                    Email = b.Email
                })]; ;
        }

        public BusinessDTO GetBusiness(long businessId) {
            return _database.Businesses.Where(b => b.Id == businessId).Select<BusinessEntity, BusinessDTO>(
                b => new() {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Phone = b.Phone,
                    Email = b.Email
                }).FirstOrDefault()!;
        }
    }
}
