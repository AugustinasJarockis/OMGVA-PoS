using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Mappers;

public class TaxMapper
{
    public static Tax FromCreateTaxRequest(CreateTaxRequest createTaxRequest)
    {
        return new Tax()
        {
            TaxType = createTaxRequest.TaxType,
            Percent = createTaxRequest.Percent
        };
    }

    public static Tax FromUpdateTaxRequest(UpdateTaxRequest updateTaxRequest, Tax tax)
    {
        tax.TaxType = updateTaxRequest.TaxType ?? tax.TaxType;
        tax.Percent = updateTaxRequest.Percent ?? tax.Percent;
        return tax;
    }

    public static TaxDTO ToDTO(Tax tax)
    {
        return new TaxDTO()
        {
            Id = tax.Id,
            TaxType = tax.TaxType,
            Percent = tax.Percent,
            IsArchived = tax.IsArchived,
        };
    }

    public static IEnumerable<TaxDTO> ToDTOs(IEnumerable<Tax> taxes)
    {
        return taxes.Select(tax => ToDTO(tax));
    }
    
}