using System.ComponentModel.DataAnnotations;

namespace OMGVA_PoS.Data_layer.DTOs
{
    public class CreateTaxRequest
    {

        [Required(ErrorMessage = "TaxType is required.")]
        [MinLength(1, ErrorMessage = "TaxType must be at least 1 character.")]
        public string TaxType { get; set; }
        
        [Required(ErrorMessage = "Percent is required.")]
        [Range(0, short.MaxValue, ErrorMessage = "Percent must be 0 or greater.")]
        public short Percent { get; set; }

        public bool IsArchived { get; set; } = false;
    }
}
