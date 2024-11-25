using OMGVA_PoS.Business_layer.Services.Security_and_Authorization;
using OMGVA_PoS.Data_layer.Context;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Repositories.Business_Management
{
    public class BusinessRepository(OMGVADbContext database, IAuthenticationRepository authenticationRepository) : IBusinessRepository
    {
        private readonly OMGVADbContext _database = database;
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        public Business CreateBusiness(CreateBusinessRequest createBusinessRequest) {
            Business business = new() {
                StripeAccId = "/////////////////////////",//TODO: somehow acquire stripe acc id
                Name = createBusinessRequest.Name,
                Address = createBusinessRequest.Address,
                Phone = createBusinessRequest.Phone,
                Email = createBusinessRequest.Email,
            };

            _database.Businesses.Add(business);
            _database.SaveChanges();
            
            createBusinessRequest.Owner.BusinessId = business.Id;
            _authenticationRepository.SignIn(createBusinessRequest.Owner);

            return business;
        }
        public List<Business> GetBusinesses() {
            return [.. _database.Businesses];
        }
    }
}
