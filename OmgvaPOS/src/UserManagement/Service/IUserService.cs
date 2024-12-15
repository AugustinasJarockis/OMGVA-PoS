using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.UserManagement.DTOs;

namespace OmgvaPOS.UserManagement.Service
{
    public interface IUserService
    {
        public UserResponse CreateUser(CreateUserRequest userRequest);
        public List<UserResponse> GetAllUsers();
        public UserResponse GetUser(long id);
        public void UpdateUser(long id, UpdateUserRequest user);
        public bool DeleteUser(long id);
        public List<UserResponse> GetBusinessUsers(long businessId);
        public List<EmployeeSchedule> GetUserSchedules(long id);
        public List<Order> GetUserOrders(long id);
        public void ValidateUserBelongsToBusiness(long? userId, long businessId);
    }
}
