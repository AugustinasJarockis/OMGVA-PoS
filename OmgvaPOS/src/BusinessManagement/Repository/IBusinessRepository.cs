using OmgvaPOS.BusinessManagement.Models;

namespace OmgvaPOS.BusinessManagement.Repository
{
    public interface IBusinessRepository
    {
        public BusinessDTO CreateBusiness(CreateBusinessRequest createBusinessRequest);
        public bool UpdateBusiness(long businessId, BusinessDTO business);
        public List<BusinessDTO> GetBusinesses();
        public BusinessDTO GetBusiness(long businessId);
    }
}
