using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Repository;

public interface ITaxRepository
{
    public Task<List<Tax>> GetAllTaxesAsync();
    public Task<Tax> GetTaxByIdAsync(long id);
    public Task<Tax> SaveTaxAsync(Tax tax);
    public Task UpdateTaxAsync(Tax tax);
    public Task DeleteTaxAsync(long id);
}