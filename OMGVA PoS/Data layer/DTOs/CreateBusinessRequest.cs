using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.DTOs
{
    public class CreateBusinessRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public SignInRequest Owner { get; set; }
    }
}
