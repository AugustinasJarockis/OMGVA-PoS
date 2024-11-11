using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class User
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
        public bool HasLeft { get; set; }
    }
}
