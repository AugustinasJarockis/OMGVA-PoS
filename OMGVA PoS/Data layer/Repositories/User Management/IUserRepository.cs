﻿using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Business_layer.Services.UserManagement
{
    public interface IUserRepository
    {
        public List<User> GetUsers();
        public User GetUser(long id);
        public void UpdateUser(long id, UpdateUserRequest user);
        public bool DeleteUser(long id);
        public List<User> GetBusinessUsers(long businessId);
        public List<EmployeeSchedule> GetUserSchedules(long id);
        public List<Order> GetUserOrders(long id);
        public long GetUserId(string username);
    }
}