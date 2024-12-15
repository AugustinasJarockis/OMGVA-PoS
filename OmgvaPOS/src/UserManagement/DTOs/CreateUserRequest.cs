using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.AuthManagement.DTOs
{
    public class CreateUserRequest
    {
        public long? BusinessId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}
