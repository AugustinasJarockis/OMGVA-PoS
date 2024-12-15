using OmgvaPOS.BusinessManagement.DTOs;

namespace OmgvaPOS.BusinessManagement.Services
{
    public interface IBusinessService
    {
        public List<BusinessDTO> GetBusinesses();
        public BusinessDTO? GetBusiness(long id);
        public BusinessDTO CreateBusiness(CreateBusinessRequest request);
        public bool UpdateBusiness(BusinessDTO business, long businessId);
    }
}
