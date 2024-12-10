using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.AuthManagement.Repository
{
    public interface IAuthenticationRepository
    {
        public User SignIn(SignInRequest signInRequest);
        public bool IsSignedIn(string email, string password);
        public bool IsEmailUsed(string email);
        public bool IsUsernamelUsed(string email);
        public Task<LoginDTO> Login(LoginRequest loginRequest);
    }
}