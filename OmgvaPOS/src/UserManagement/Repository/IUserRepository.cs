using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.UserManagement.Repository
{
    public interface IUserRepository
    {
        public User CreateUser(User user);
        public List<User> GetUsers();
        public User GetUser(long id); 
        public User? GetUserByUsername(string username);
        public User? GetUserNoException(long id);
        public void UpdateUser(long id, UpdateUserRequest user);
        public bool DeleteUser(long id);
        public List<User> GetBusinessUsers(long businessId);
        public List<EmployeeSchedule> GetUserSchedules(long id);
        public List<Order> GetUserOrders(long id);
        public long GetUserId(string username);
        public bool AnyUserEmailDuplicate(string email);
        public bool AnyUserUsernameDuplicate(string username);
    }
}