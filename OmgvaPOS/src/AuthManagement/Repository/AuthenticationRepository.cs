using OmgvaPOS.Database.Context;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.AuthManagement.Repository
{
    public class AuthenticationRepository(OmgvaDbContext database) : IAuthenticationRepository
    {
        private readonly OmgvaDbContext _database = database;

        public User SignUpUser(User user)
        {
            try
            {
                var createdUser = _database.Users.Add(user).Entity;
                _database.SaveChanges();
                return createdUser;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during sign-in.", ex);
            }
        }

        public User GetUserByUsername(string username)
        {
            var user = _database.Users.FirstOrDefault(u => u.Username.Equals(username)) ?? throw new KeyNotFoundException("User not found.");
            return user;
        }

        public bool AnyUserDuplicate(string username, string password)
        {
            return _database.Users.Where(u => u.Username == username && u.Password == password).Any();
        }

        public bool AnyUserEmailDuplicate(string email)
        {
            return _database.Users.Any(u => u.Email == email);
        }

        public bool AnyUserUsernameDuplicate(string username)
        {
            return _database.Users.Any(u => u.Username == username);
        }
    }
}
