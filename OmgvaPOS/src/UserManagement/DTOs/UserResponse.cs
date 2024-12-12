using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.UserManagement.DTOs
{
    public class UserResponse
    {
        public long? Id { get; set; }
        public long? BusinessId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
        public bool? HasLeft { get; set; }

    }
}
