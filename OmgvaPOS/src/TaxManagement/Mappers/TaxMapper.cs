using OmgvaPOS.TaxManagement.DTOs;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Mappers;

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

    public static TaxDto ToDTO(Tax tax)
    {
        return new TaxDto()
        {
            Id = tax.Id,
            TaxType = tax.TaxType,
            Percent = tax.Percent,
            IsArchived = tax.IsArchived,
        };
    }

    public static IEnumerable<TaxDto> ToDTOs(IEnumerable<Tax> taxes)
    {
        return taxes.Select(tax => ToDTO(tax));
    }
    
}