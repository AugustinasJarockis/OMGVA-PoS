using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.Exceptions;

namespace OmgvaPOS.DiscountManagement.Service;

public class DiscountValidatorService
{
    private IDiscountRepository _discountRepository;

    public DiscountValidatorService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public void ValidateDiscountBelongsToBusiness(long? discountId, long businessId)
    {
        if (discountId == null) 
            return;
            
        var discount = _discountRepository.GetDiscount((long) discountId);
        if (discount == null)
            throw new NotFoundException("There is no such discount available.");

        if (businessId != discount.BusinessId)
            throw new ForbiddenException();
    }
}