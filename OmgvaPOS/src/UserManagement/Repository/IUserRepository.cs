using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.UserManagement.Repository
{
    public interface IUserRepository
    {
        public List<User> GetUsers();
        public User GetUser(long id); 
        public User? GetUserNoExcept(long id);
        public void UpdateUser(long id, UpdateUserRequest user);
        public bool DeleteUser(long id);
        public List<User> GetBusinessUsers(long businessId);
        public List<EmployeeSchedule> GetUserSchedules(long id);
        public List<OrderManagement.Models.Order> GetUserOrders(long id);
        public long GetUserId(string username);
    }
}