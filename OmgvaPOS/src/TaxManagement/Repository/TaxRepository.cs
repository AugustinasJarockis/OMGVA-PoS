using Microsoft.EntityFrameworkCore;
using OmgvaPOS.Database.Context;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.TaxManagement.Repository;

public class TaxRepository : ITaxRepository
{
    private readonly OmgvaDbContext _database;
    private readonly ILogger<TaxRepository> _logger;

    public TaxRepository(OmgvaDbContext context, ILogger<TaxRepository> logger)
    {
        _database = context;
        _logger = logger;
    }

    public List<Tax> GetAllTaxes()
    {
        try
        {
            return _database.Taxes.Where(t => t.IsArchived == false).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all taxes.");
            throw new ApplicationException("Error retrieving taxes.");
        }
    }

    public Tax GetTaxById(long id)
    {
        try
        {
            return _database.Taxes.Where(tax => tax.Id == id).FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving tax with ID {id}.");
            throw new ApplicationException("Error retrieving tax.");
        }
    }

    public Tax SaveTax(Tax tax)
    {
        try
        {
            _database.Taxes.Add(tax);
            _database.SaveChanges();
            return tax;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving tax.");
            throw new ApplicationException("An unexpected error occurred while saving tax.");
        }
    }

    public Tax UpdateTax(Tax tax)
    {
        try
        {
            var newTax = (Tax)tax.Clone();
            _database.Add(newTax);
            _database.Entry(tax).CurrentValues.SetValues(_database.Entry(tax).OriginalValues);
            tax.IsArchived = true;
            _database.Taxes.Update(tax);
            _database.SaveChanges();
            return newTax;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating tax with ID {tax.Id}.");
            throw new ApplicationException("Error updating tax.");
        }
    }

    public void DeleteTax(long id)
    {
        try
        {
            var tax = _database.Taxes.Find(id);
            if (tax != null)
            {
                tax.IsArchived = true;
                _database.Taxes.Update(tax);
                _database.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting tax with ID {id}.");
            throw new ApplicationException("Error deleting tax.");
        }
    }
}
