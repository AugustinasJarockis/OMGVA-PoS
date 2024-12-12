using OmgvaPOS.DiscountManagement.Models;

namespace OmgvaPOS.src.DiscountManagement.Repository;

public interface IDiscountRepository
{
    public Discount AddDiscount(Discount discount);
    public List<Discount> GetGlobalDiscounts();
    public List<Discount> GetBusinessDiscounts(long businessId);
    public Discount GetDiscount(long id);
    public void UpdateDiscount(Discount discount);
}
