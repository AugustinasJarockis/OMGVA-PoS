using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.AuthManagement.Repository
{
    public interface IAuthenticationRepository
    {
        public User SignUpUser(User user);
        public User GetUserByUsername(string username);
        public bool AnyUserDuplicate(string username, string password);
        public bool AnyUserEmailDuplicate(string email);
        public bool AnyUserUsernameDuplicate(string username);
    }
}