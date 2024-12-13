using OmgvaPOS.DiscountManagement.Models;

namespace OmgvaPOS.DiscountManagement.Repository;

public interface IDiscountRepository
{
    public Discount AddDiscount(Discount discount);
    public List<Discount> GetBusinessDiscounts(long businessId);
    public Discount GetDiscount(long id);
    public void UpdateDiscountValidUntil(Discount discount);
    public void ArchiveDiscount(Discount discount);
}
