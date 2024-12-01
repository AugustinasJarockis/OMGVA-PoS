using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.DTOs
{
    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
