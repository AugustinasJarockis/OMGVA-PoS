using OmgvaPOS.TaxManagement.Entities;

namespace OmgvaPOS.TaxManagement.Repository;

public interface ITaxRepository
{
    public Task<List<TaxEntity>> GetAllTaxesAsync();
    public Task<TaxEntity> GetTaxByIdAsync(long id);
    public Task<TaxEntity> SaveTaxAsync(TaxEntity taxEntity);
    public Task UpdateTaxAsync(TaxEntity taxEntity);
    public Task DeleteTaxAsync(long id);
}