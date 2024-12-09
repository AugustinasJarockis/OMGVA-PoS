using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Repositories.Business_Management
{
    public interface IBusinessRepository
    {
        public BusinessDTO CreateBusiness(CreateBusinessRequest createBusinessRequest);
        public bool UpdateBusiness(long businessId, BusinessDTO business);
        public List<BusinessDTO> GetBusinesses();
        public BusinessDTO GetBusiness(long businessId);
    }
}
