using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.TaxManagement.Entities;

namespace OmgvaPOS.TaxManagement.Repository;

public class TaxRepository : ITaxRepository
{
    private readonly OmgvaDbContext _context;
    private readonly ILogger<TaxRepository> _logger;

    public TaxRepository(OmgvaDbContext context, ILogger<TaxRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaxEntity>> GetAllTaxesAsync()
    {
        try
        {
            return await _context.Taxes.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all taxes.");
            throw new ApplicationException("Error retrieving taxes.");
        }
    }

    public async Task<TaxEntity> GetTaxByIdAsync(long id)
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

    public async Task<TaxEntity> SaveTaxAsync(TaxEntity taxEntity)
    {
        try
        {
            _context.Taxes.Add(taxEntity);
            await _context.SaveChangesAsync();
            return taxEntity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving tax.");
            throw new ApplicationException("An unexpected error occurred while saving tax.");
        }
    }

    public async Task UpdateTaxAsync(TaxEntity taxEntity)
    {
        try
        {
            _context.Taxes.Update(taxEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating tax with ID {taxEntity.Id}.");
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
