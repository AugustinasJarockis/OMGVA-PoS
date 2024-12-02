using System.ComponentModel.DataAnnotations;

namespace OMGVA_PoS.Data_layer.DTOs
{
    public class UpdateTaxRequest
    {
        
        [MinLength(1, ErrorMessage = "TaxType must be at least 1 character.")]
        public string? TaxType { get; set; }
        
        [Range(0, short.MaxValue, ErrorMessage = "Percent must be 0 or greater.")]
        public short? Percent { get; set; }

        public bool? IsArchived { get; set; }
    }
}
