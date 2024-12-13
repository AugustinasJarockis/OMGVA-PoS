using OmgvaPOS.Database.Context;
using OmgvaPOS.DiscountManagement.Enums;
using OmgvaPOS.DiscountManagement.Models;

namespace OmgvaPOS.DiscountManagement.Repository;

public class DiscountRepository : IDiscountRepository
{
    private readonly OmgvaDbContext _context;
    private readonly ILogger<DiscountRepository> _logger;

    public DiscountRepository(OmgvaDbContext context, ILogger<DiscountRepository> logger) {
        _context = context;
        _logger = logger;
    }
    public Discount AddDiscount(Discount discount) {
        try {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
            return discount;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while saving discount.");
            throw new ApplicationException("Error saving discount.");
        }
    }

    public List<Discount> GetBusinessDiscounts(long businessId) {
        try {
            return [.. _context.Discounts
                .Where(d => d.BusinessId == businessId)
                .Where(d => d.Type == DiscountType.Item)
                .Where(d => d.IsArchived == false)];
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while fetching all discounts.");
            throw new ApplicationException("Error fetching all discounts.");
        }
    }

    public Discount GetDiscount(long id) {
        try {
            return _context.Discounts.Where(d => d.Id == id).FirstOrDefault();
        }
        catch (Exception ex) {
            _logger.LogError(ex, $"An error occurred while retrieving item with ID {id}.");
            throw new ApplicationException("Error retrieving item.");
        }
    }

    public void UpdateDiscount(Discount discount) {
        var existingDiscount = _context.Discounts.FirstOrDefault(d => d.Id == discount.Id);
        // thats all we can update
        existingDiscount.TimeValidUntil = discount.TimeValidUntil;
        _context.SaveChanges();
    }
}
