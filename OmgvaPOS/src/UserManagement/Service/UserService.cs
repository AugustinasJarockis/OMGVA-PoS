using OmgvaPOS.UserManagement.Repository;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.AuthManagement.Repository;
using OmgvaPOS.UserManagement.Mappers;

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

            var user = UserMapper.FromSignUpRequest(userRequest);

            var createdUser = _authenticationRepository.SignUpUser(user);

            if (createdUser == null)
                throw new ArgumentNullException(nameof(userRequest));

            return UserMapper.FromUser(createdUser);
        }
        public List<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetUsers();
            var usersResponse = new List<UserResponse>();
            foreach (var user in users)
            {
                usersResponse.Add(UserMapper.FromUser(user));
            }
            return usersResponse;
        }
        public UserResponse GetUser(long id)
        {
            var user = _userRepository.GetUser(id);
           
            return UserMapper.FromUser(user);
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
                usersResponse.Add(UserMapper.FromUser(user));
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
