using OmgvaPOS.Order.Entities;
using OmgvaPOS.Schedule.Entities;
using OmgvaPOS.UserManagement.Models;
using OmgvaPOS.UserManagement.Entities;

namespace OmgvaPOS.UserManagement.Repository
{
    public interface IUserRepository
    {
        public List<UserEntity> GetUsers();
        public UserEntity GetUser(long id);
        public void UpdateUser(long id, UpdateUserRequest user);
        public bool DeleteUser(long id);
        public List<UserEntity> GetBusinessUsers(long businessId);
        public List<EmployeeScheduleEntity> GetUserSchedules(long id);
        public List<OrderEntity> GetUserOrders(long id);
        public long GetUserId(string username);
    }
}