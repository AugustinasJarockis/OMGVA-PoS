﻿using OmgvaPOS.Database.Context;
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
        _context.Discounts.Add(discount);
        _context.SaveChanges();
        return discount;
    }

    public List<Discount> GetBusinessDiscounts(long businessId) {
        return [.. _context.Discounts
            .Where(d => d.BusinessId == businessId)
            .Where(d => d.Type == DiscountType.Item)
            .Where(d => d.IsArchived == false)];
    }

    public Discount? GetDiscount(long id) {
        return _context.Discounts
            .FirstOrDefault(d => d.Id == id);
    }

    public void UpdateDiscount(Discount discount) {
        _context.Discounts.Update(discount);
        _context.SaveChanges();
    }
}
