using System.ComponentModel.DataAnnotations;
using OmgvaPOS.BusinessManagement.Models;

namespace OmgvaPOS.PaymentManagement.Models
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
