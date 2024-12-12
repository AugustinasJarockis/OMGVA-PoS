using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.AuthManagement.Service
{
    public interface IAuthService
    {
        public User SignUp(SignUpRequest signUpRequest);
        public bool IsSignedUp(string username, string password);
        public bool IsEmailUsed(string email);
        public bool IsUsernameUsed(string username);
        public LoginDTO Login(LoginRequest loginRequest);
        public LoginDTO GenerateAdminJwtToken(long businessId, TokenDetailsDTO tokenDetails);
    }
}