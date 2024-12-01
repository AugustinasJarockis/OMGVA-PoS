using OMGVA_PoS.Data_layer.Context;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Business_layer.Services.UserManagement
{
    public class UserRepository(OMGVADbContext database) : IUserRepository
    {
        private readonly OMGVADbContext _database = database;

        public List<User> GetUsers()
        {
            try
            {
                return [.. _database.Users];
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving users.", ex);
            }
        }

        public User GetUser(long id)
        {
            try
            {
                return _database.Users.FirstOrDefault(u => u.Id == id)
                    ?? throw new KeyNotFoundException("User not found.");
        }
            catch (Exception ex)
        {
                throw new ApplicationException("Error retrieving the user.", ex);
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
                userToUpdate.Password = !string.IsNullOrEmpty(user.Password)
                    ? BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13)
                    : userToUpdate.Password;

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
    }
}