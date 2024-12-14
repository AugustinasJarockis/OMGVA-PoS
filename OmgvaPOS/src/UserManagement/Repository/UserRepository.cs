using OmgvaPOS.Database.Context;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.UserManagement.Repository
{
    public class UserRepository(OmgvaDbContext database) : IUserRepository
    {
        private readonly OmgvaDbContext _database = database;

        public User CreateUser(User user)
        {
            _database.Users.Add(user);
            _database.SaveChanges();
            return user;
        }

        public List<User> GetUsers()
        {
            return [.. _database.Users];
        }

        public User GetUser(long id)
        {
            try {
                return _database.Users.FirstOrDefault(u => u.Id == id)
                    ?? throw new KeyNotFoundException("User not found.");
            }
            catch (Exception ex) {
                throw new ApplicationException("Error retrieving the user.", ex);
            }
        }
        
        public User? GetUserByUsername(string username)
        {
            var user = _database.Users
                .FirstOrDefault(u => u.Username.Equals(username));
            return user;
        }
        
        public User? GetUserNoException(long id) {
            try {
                return _database.Users.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception) {
                return null;
            }
        }
        public void UpdateUser(long id, UpdateUserRequest user)
        {
            try
            {
                var userToUpdate = _database.Users.SingleOrDefault(u => u.Id == id);
                if (userToUpdate == null)
                    throw new KeyNotFoundException("User not found.");

                userToUpdate.Name = user.Name ?? userToUpdate.Name;
                userToUpdate.Username = user.Username ?? userToUpdate.Username;
                userToUpdate.Email = user.Email ?? userToUpdate.Email;
                userToUpdate.Role = user.Role ?? userToUpdate.Role;
                userToUpdate.Password = user.Password ?? userToUpdate.Password;

                _database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating the user.", ex);
            }
        }

        public bool DeleteUser(long id)
        {
            try
            {
                var user = _database.Users.SingleOrDefault(u => u.Id == id);
                if (user == null)
                    return false;

                user.HasLeft = true;
                _database.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting the user.", ex);
            }
        }

        public List<User> GetBusinessUsers(long businessId)
        {
            try
            {
                return [.. _database.Users.Where(u => u.BusinessId == businessId)];
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving business users.", ex);
            }
        }
        public List<EmployeeSchedule> GetUserSchedules(long id)
        {
            try
            {
                return [.. _database.EmployeeSchedules.Where(s => s.Id == id)];
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving user schedules.", ex);
            }
        }
        public List<Order> GetUserOrders(long id)
        {
            try
            {
                return [.. _database.Orders.Where(o => o.Id == id)];
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving user orders.", ex);
            }
        }
        public long GetUserId(string username)
        {
            try
            {
                var user = _database.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null)
                    throw new KeyNotFoundException("User not found.");
                return user.Id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving user ID.", ex);
            }
        }
        
        public bool AnyUserEmailDuplicate(string email)
        {
            return _database.Users
                .Any(u => u.Email == email);
        }

        public bool AnyUserUsernameDuplicate(string username)
        {
            return _database.Users
                .Any(u => u.Username == username);
        }
    }
}