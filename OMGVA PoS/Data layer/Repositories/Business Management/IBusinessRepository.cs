using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Repositories.Business_Management
{
    public interface IBusinessRepository
    {
        public Business CreateBusiness(CreateBusinessRequest createBusinessRequest);
        public List<Business> GetBusinesses();
    }
}
