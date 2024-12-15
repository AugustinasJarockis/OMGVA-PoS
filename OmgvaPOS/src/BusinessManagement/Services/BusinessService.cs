using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Mappers;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.BusinessManagement.Repository;
using OmgvaPOS.BusinessManagement.Validator;
using OmgvaPOS.Exceptions;

namespace OmgvaPOS.BusinessManagement.Services
{
    public class BusinessService(IBusinessRepository businessRepository) : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository = businessRepository;

        public List<BusinessDTO> GetBusinesses() {
            return _businessRepository.GetBusinesses()
                .Select(b => b.ToBusinessDTO())
                .ToList();
        }
        public BusinessDTO? GetBusiness(long id)
        {
            var business = _businessRepository.GetBusiness(id);
            return business?.ToBusinessDTO();
        }
        public BusinessDTO CreateBusiness(CreateBusinessRequest request)
        {
            var createdBusiness = _businessRepository.CreateBusiness(request.ToBusiness());
            return createdBusiness.ToBusinessDTO();
        }
        
        public bool UpdateBusiness(BusinessDTO business, long businessId)
        {
            BusinessValidator.ValidateBusinessDTO(business);
            var businessToUpdate = _businessRepository.GetBusiness(businessId);
            if (businessToUpdate == null)
                throw new NotFoundException();
            
            businessToUpdate = business.ToBusiness(businessToUpdate);
            return _businessRepository.UpdateBusiness(businessToUpdate);
        }
    }
}
