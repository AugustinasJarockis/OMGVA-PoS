using OmgvaPOS.BusinessManagement.Models;

namespace OmgvaPOS.BusinessManagement.Repository
{
    public interface IBusinessRepository
    {
        public List<Business> GetBusinesses();
        public Business GetBusiness(long businessId);
        public Business CreateBusiness(Business createBusinessRequest);
        public bool UpdateBusiness(Business business);
    }
}
