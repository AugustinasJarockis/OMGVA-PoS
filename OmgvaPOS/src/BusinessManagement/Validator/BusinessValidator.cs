using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.Exceptions;
using OmgvaPOS.Validators;

namespace OmgvaPOS.BusinessManagement.Validator;

public static class BusinessValidator
{
    public static void ValidateBusinessDTO(BusinessDTO businessDTO)
    {
        if (!businessDTO.Email?.IsValidEmail() ?? false)
            throw new BadRequestException("Email is not valid");
        
        if (!businessDTO.Phone?.IsValidPhone() ?? false)
            throw new BadRequestException("Phone is not valid");
    }
}