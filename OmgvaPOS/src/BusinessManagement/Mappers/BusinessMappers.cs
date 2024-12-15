using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Models;

namespace OmgvaPOS.BusinessManagement.Mappers
{
    public static class BusinessMappers
    {
        public static Business ToBusiness(this CreateBusinessRequest request) {
            return new Business() {
                StripeAccId = "/////////////////////////",//TODO: somehow acquire stripe acc id
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email
            };
        }

        public static Business ToBusiness(this BusinessDTO business, Business businessBase) {
            businessBase.Id = businessBase.Id;
            businessBase.StripeAccId = businessBase.StripeAccId;
            businessBase.Name = business.Name ?? businessBase.Name;
            businessBase.Address = business.Address ?? businessBase.Address;
            businessBase.Phone = business.Phone ?? businessBase.Phone;
            businessBase.Email = business.Email ?? businessBase.Email;
            return businessBase;
        }

        public static BusinessDTO ToBusinessDTO(this Business business)
        {
            return new BusinessDTO() {
                Id = business.Id,
                Name = business.Name,
                Address = business.Address,
                Phone = business.Phone,
                Email = business.Email
            };
        }
    }
}
