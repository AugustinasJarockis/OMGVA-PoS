using OmgvaPOS.UserManagement.Repository;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.UserManagement.Models;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.AuthManagement.Repository;

namespace OmgvaPOS.UserManagement.Service
{
    public class UserService(IUserRepository userRepository, IAuthenticationRepository authenticationRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        public UserResponse CreateUser(SignUpRequest userRequest)
        {
            if (userRequest == null)
                throw new ArgumentNullException(nameof(userRequest));

            User user = new()
            {
                Name = userRequest.Name,
                Username = userRequest.Username,
                Email = userRequest.Email,
                Role = userRequest.Role,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userRequest.Password, 13),
                BusinessId = userRequest.BusinessId,
                HasLeft = false
            };

            var createdUser = _authenticationRepository.SignUpUser(user);

            if (createdUser == null)
                throw new ArgumentNullException(nameof(userRequest));

            UserResponse userResponse = new()
            {
                Id = createdUser.Id,
                BusinessId = createdUser.BusinessId,
                Name = createdUser.Name,
                Username = createdUser.Username,
                Email = createdUser.Email,
                Role = createdUser.Role,
                HasLeft = createdUser.HasLeft
            };

            return userResponse;
        }
        public List<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetUsers();
            var usersResponse = new List<UserResponse>();
            foreach (var user in users)
            {
                UserResponse userResponse = new()
                {
                    Id = user.Id,
                    BusinessId = user.BusinessId,
                    Name = user.Name,
                    Username = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    HasLeft = user.HasLeft
                };
                usersResponse.Add(userResponse);
            }
            return usersResponse;
        }
        public UserResponse GetUser(long id)
        {
            var user = _userRepository.GetUser(id);
            UserResponse userResponse = new()
            {
                Id = user.Id,
                BusinessId = user.BusinessId,
                Name = user.Name,
                Username = user.Name,
                Email = user.Email,
                Role = user.Role,
                HasLeft = user.HasLeft
            };
            return userResponse;
        }
        public void UpdateUser(long id, UpdateUserRequest user)
        {
            if (user.Username != null && _authenticationRepository.AnyUserUsernameDuplicate(user.Username))
                throw new ArgumentException(nameof(user.Username));

            user.Password = !string.IsNullOrEmpty(user.Password)
                    ? BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13)
                    : null;
            _userRepository.UpdateUser(id, user);
        }
        public bool DeleteUser(long id)
        {
            return _userRepository.DeleteUser(id);
        }
        public List<UserResponse> GetBusinessUsers(long businessId)
        {
            var users = _userRepository.GetBusinessUsers(businessId);
            var usersResponse = new List<UserResponse>();
            foreach (var user in users)
            {
                UserResponse userResponse = new()
                {
                    Id = user.Id,
                    BusinessId = user.BusinessId,
                    Name = user.Name,
                    Username = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    HasLeft = user.HasLeft
                };
                usersResponse.Add(userResponse);
            }
            return usersResponse;
        }
        public List<EmployeeSchedule> GetUserSchedules(long id)
        {
            return _userRepository.GetUserSchedules(id);
        }
        public List<Order> GetUserOrders(long id)
        {
            return _userRepository.GetUserOrders(id);
        }
    }
}
