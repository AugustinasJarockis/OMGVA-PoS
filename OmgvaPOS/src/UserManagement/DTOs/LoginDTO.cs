﻿namespace OmgvaPOS.UserManagement.DTOs
{
    public class LoginDTO(bool isSuccess, string message, string? token = null)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public string Message { get; set; } = message;
        public string? Token { get; set; } = token;
    }
}
