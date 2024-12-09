using System.ComponentModel.DataAnnotations;
using OmgvaPOS.BusinessManagement.Entities;

namespace OmgvaPOS.Payment.Entities
{
    public class StripeReaderEntity
    {
        [Key]
        public string ReaderId { get; set; }
        public long BusinessId { get; set; }
        
        // navigational properties
        // for foreign keys
        public BusinessEntity BusinessEntity { get; set; }
    }
}
