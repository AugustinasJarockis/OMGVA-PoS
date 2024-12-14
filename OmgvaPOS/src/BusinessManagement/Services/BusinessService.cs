using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Mappers;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.BusinessManagement.Repository;
using OmgvaPOS.BusinessManagement.Validator;

namespace OmgvaPOS.BusinessManagement.Services
{
    public class BusinessService(IBusinessRepository businessRepository) : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository = businessRepository;
        public List<BusinessDTO> GetBusinesses() {
            return _businessRepository.GetBusinesses().Select(b => b.ToBusinessDTO()).ToList();
        }
        public BusinessDTO? GetBusiness(long id) {
            return _businessRepository.GetBusiness(id).ToBusinessDTO();
        }
        public BusinessDTO CreateBusiness(CreateBusinessRequest request) {
            return _businessRepository.CreateBusiness(request.ToBusiness()).ToBusinessDTO();
        }
        public bool UpdateBusiness(BusinessDTO business)
        {
            BusinessValidator.ValidateBusinessDTO(business);
            Business businessToUpdate = _businessRepository.GetBusiness((long)business.Id); //TODO: Unlikely but potential error here 
            businessToUpdate = business.ToBusiness(businessToUpdate);
            return _businessRepository.UpdateBusiness(businessToUpdate);
        }
    }
}
