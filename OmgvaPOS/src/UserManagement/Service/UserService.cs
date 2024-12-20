﻿using OmgvaPOS.UserManagement.Repository;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.Exceptions;
using OmgvaPOS.UserManagement.Mappers;
using OmgvaPOS.UserManagement.Validators;

namespace OmgvaPOS.UserManagement.Service
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        
        public UserResponse CreateUser(CreateUserRequest userRequest)
        {
            UserValidator.ValidateCreateUserRequest(userRequest);
            ValidateUserSignUpConflicts(userRequest);

            var user = userRequest.ToUser();
            var createdUser = _userRepository.CreateUser(user);
            
            return createdUser.ToUserResponse();
        }
        public List<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetUsers();
            var usersResponse = new List<UserResponse>();
            foreach (var user in users)
            {
                usersResponse.Add(user.ToUserResponse());
            }
            return usersResponse;
        }
        public UserResponse GetUser(long id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                throw new NotFoundException();
            
            return user.ToUserResponse();
        }
        public void UpdateUser(long id, UpdateUserRequest user)
        {
            if (user.Username != null && IsUsernameUsed(user.Username))
                throw new ConflictException(nameof(user.Username));

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
                usersResponse.Add(user.ToUserResponse());
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
        
        public void ValidateUserBelongsToBusiness(long? userId, long businessId)
        {
            if (userId == null) 
                return;
            
            var user = _userRepository.GetUser((long) userId);
                
            if (user == null)
                throw new NotFoundException("Could not find user");

            if (businessId != user.BusinessId)
                throw new ForbiddenException("There is no such user that works in this business.");
        }
        
        private void ValidateUserSignUpConflicts(CreateUserRequest createUserRequest)
        {
            if (IsEmailUsed(createUserRequest.Email))
            {
                throw new ConflictException("This email is already in use.");
            }

            if (IsUsernameUsed(createUserRequest.Username))
            {
                throw new ConflictException("This username is already in use.");
            }
        }
        
        private bool IsEmailUsed(string email)
        {
            return _userRepository.AnyUserEmailDuplicate(email);
        }

        private bool IsUsernameUsed(string username)
        {
            return _userRepository.AnyUserUsernameDuplicate(username);
        }
    }
}
