using Microsoft.EntityFrameworkCore;
using OMGVA_PoS.Data_layer.Context;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Repositories.TaxManagement;

public class TaxRepository : ITaxRepository
{
    private readonly OMGVADbContext _context;
    private readonly ILogger<TaxRepository> _logger;

    public TaxRepository(OMGVADbContext context, ILogger<TaxRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Tax>> GetAllTaxesAsync()
    {
        try
        {
            return await _context.Taxes.Where(t => t.IsArchived == false).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all taxes.");
            throw new ApplicationException("Error retrieving taxes.");
        }
    }

    public async Task<Tax> GetTaxByIdAsync(long id)
    {
        try
        {
            return await _context.Taxes.Where(tax => tax.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving tax with ID {id}.");
            throw new ApplicationException("Error retrieving tax.");
        }
    }

    public async Task<Tax> SaveTaxAsync(Tax tax)
    {
        try
        {
            _context.Taxes.Add(tax);
            await _context.SaveChangesAsync();
            return tax;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving tax.");
            throw new ApplicationException("An unexpected error occurred while saving tax.");
        }
    }

    public async Task UpdateTaxAsync(Tax tax)
    {
        try
        {
            _context.Taxes.Update(tax);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating tax with ID {tax.Id}.");
            throw new ApplicationException("Error updating tax.");
        }
    }

    public async Task DeleteTaxAsync(long id)
    {
        try
        {
            var tax = await _context.Taxes.FindAsync(id);
            if (tax != null)
            {
                tax.IsArchived = true;
                _context.Taxes.Update(tax);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting tax with ID {id}.");
            throw new ApplicationException("Error deleting tax.");
        }
    }
}
