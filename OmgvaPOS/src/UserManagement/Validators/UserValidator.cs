using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.Exceptions;
using OmgvaPOS.Validators;

namespace OmgvaPOS.UserManagement.Validators;

public static class UserValidator
{
    public static void ValidateCreateUserRequest(CreateUserRequest createUserRequest)
    {
        if (createUserRequest == null)
            throw new BadRequestException("Bad request");
        
        if (!createUserRequest.Email.IsValidEmail())
            throw new BadRequestException("Email is not valid.");

        if (!createUserRequest.Name.IsValidName())
            throw new BadRequestException("Name is not valid.");

        if (!createUserRequest.Username.IsValidUsername())
            throw new BadRequestException("Username is not valid.");

        if (!createUserRequest.Password.IsValidPassword())
            throw new BadRequestException("Password is not valid.");
    }
}