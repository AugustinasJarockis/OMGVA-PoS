using OmgvaPOS.AuthManagement.DTOs;

namespace OmgvaPOS.AuthManagement.Service
{
    public interface IAuthService
    {
        public LoginResponseDTO Login(LoginRequest loginRequest);
        public LoginResponseDTO LoginAdminWithDifferentBusiness(long businessId, TokenDetailsDTO tokenDetails);
    }
}