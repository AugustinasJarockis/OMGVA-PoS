using OmgvaPOS.Database.Context;
using OmgvaPOS.Exceptions;
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

        public User? GetUser(long id)
        {
            return _database.Users.FirstOrDefault(u => u.Id == id);
        }
        
        public User? GetUserByUsername(string username)
        {
            var user = _database.Users
                .FirstOrDefault(u => u.Username.Equals(username));
            return user;
        }
        
        public User? GetUserNoException(long id) {
            return _database.Users.FirstOrDefault(u => u.Id == id);
        }
        public void UpdateUser(long id, UpdateUserRequest user)
        {
            var userToUpdate = _database.Users.SingleOrDefault(u => u.Id == id);
            if (userToUpdate == null)
                throw new NotFoundException("User not found.");

            userToUpdate.Name = user.Name ?? userToUpdate.Name;
            userToUpdate.Username = user.Username ?? userToUpdate.Username;
            userToUpdate.Email = user.Email ?? userToUpdate.Email;
            userToUpdate.Role = user.Role ?? userToUpdate.Role;
            userToUpdate.Password = user.Password ?? userToUpdate.Password;

            _database.SaveChanges();
        }

        public bool DeleteUser(long id)
        {
            var user = _database.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            user.HasLeft = true;
            _database.SaveChanges();
            return true;
        }

        public List<User> GetBusinessUsers(long businessId)
        {
            return [.. _database.Users.Where(u => u.BusinessId == businessId)];
        }
        public List<EmployeeSchedule> GetUserSchedules(long id)
        {
            return [.. _database.EmployeeSchedules.Where(s => s.Id == id)];
        }
        public List<Order> GetUserOrders(long id)
        {
            return [.. _database.Orders.Where(o => o.Id == id)];
        }
        public long? GetUserId(string username)
        {
            var user = _database.Users.FirstOrDefault(u => u.Username.Equals(username));
            return user?.Id;
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