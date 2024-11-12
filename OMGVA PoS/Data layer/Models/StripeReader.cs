using System.ComponentModel.DataAnnotations;

namespace OMGVA_PoS.Data_layer.Models
{
    public class StripeReader
    {
        [Key]
        public string ReaderId { get; set; }
        public long BusinessId { get; set; }
        
        // navigational properties
        // for foreign keys
        public Business Business { get; set; }
    }
}
