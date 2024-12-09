using OmgvaPOS.UserManagement.Entities;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.Auth.Repository
{
    public interface IAuthenticationRepository
    {
        public UserEntity SignIn(SignInRequest signInRequest);
        public bool IsSignedIn(string email, string password);
        public bool IsEmailUsed(string email);
        public bool IsUsernamelUsed(string email);
        public Task<LoginDTO> Login(LoginRequest loginRequest);
    }
}