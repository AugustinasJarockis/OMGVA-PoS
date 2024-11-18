using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Helper_modules.Utilities;

namespace OMGVA_PoS.Business_layer.Services.Security_and_Authorization
{
    public interface IUserAuthenticationRepository
    {
        public User SignIn(SignInModel signInModel);
        public bool IsSignedIn(string email, string password);
        public bool IsEmailUsed(string email);
        public bool IsUsernamelUsed(string email);
        public Task<LoginDTO> Login(LoginModel loginModel);
    }
}