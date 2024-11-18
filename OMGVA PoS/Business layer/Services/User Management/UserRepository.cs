using OMGVA_PoS.Data_layer.Context;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Business_layer.Services.UserManagement
{
    public class UserRepository(OMGVADbContext database) : IUserRepository
    {
        private readonly OMGVADbContext _database = database;

        public List<User> GetUsers()
        {
            return [.. _database.Users];
        }

        public User GetUser(long id)
        {
            return _database.Users.Where(u => u.Id == id).First();
        }
        public void UpdateUser(long id, User user)
        {
            var userToUpdate = _database.Users.SingleOrDefault(u => u.Id == id);
            if (userToUpdate != null)
            {
                userToUpdate.Name = user.Name;
                userToUpdate.Username = user.Username;
                userToUpdate.Email = user.Email;
                userToUpdate.Role = user.Role;
                userToUpdate.Password = user.Password;
                userToUpdate.BusinessId = user.BusinessId;
                _database.SaveChanges();
            }
        }

        //galimai deleta istrint
        public void DeleteUser(long id)
        {
            var user = _database.Users.SingleOrDefault(u => u.Id == id);
            if(user != null)
                _database.Users.Remove(user);
        }
        public List<User> GetBusinessUsers(long businessId)
        {
            return [.. _database.Users.Where(u => u.BusinessId == businessId)];
        }
        public List<EmployeeSchedule> GetUserSchedules(long id)
        {
            return [.. _database.EmployeeSchedules.Where(u => u.Id == id)];
        }
        public List<Order> GetUserOrders(long id)
        {
            return [.. _database.Orders.Where(u => u.Id == id)];
        }
        public long GetUserId(string username)
        {
            return _database.Users.Where(u => u.Username.Equals(username)).First().Id;
        }
    }
}