using OmgvaPOS.TaxManagement.Entities;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Mappers;

public class TaxMapper
{
    public static TaxEntity FromCreateTaxRequest(CreateTaxRequest createTaxRequest)
    {
        return new TaxEntity()
        {
            TaxType = createTaxRequest.TaxType,
            Percent = createTaxRequest.Percent
        };
    }

    public static TaxEntity FromUpdateTaxRequest(UpdateTaxRequest updateTaxRequest, TaxEntity taxEntity)
    {
        taxEntity.TaxType = updateTaxRequest.TaxType ?? taxEntity.TaxType;
        taxEntity.Percent = updateTaxRequest.Percent ?? taxEntity.Percent;
        return taxEntity;
    }

    public static TaxDTO ToDTO(TaxEntity taxEntity)
    {
        return new TaxDTO()
        {
            Id = taxEntity.Id,
            TaxType = taxEntity.TaxType,
            Percent = taxEntity.Percent,
            IsArchived = taxEntity.IsArchived,
        };
    }

    public static IEnumerable<TaxDTO> ToDTOs(IEnumerable<TaxEntity> taxes)
    {
        return taxes.Select(tax => ToDTO(tax));
    }
    
}