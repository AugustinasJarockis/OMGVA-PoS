using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.AuthManagement.DTOs
{
    public class TokenDetailsDTO
    {
        public long UserId { get; set; }
        public UserRole UserRole { get; set; }
        public string UserName { get; set; }
        public long BusinessId { get; set; }
    }
}
