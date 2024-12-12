using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;
using System.Data;

namespace OmgvaPOS.UserManagement.Mappers
{
    public class UserMapper
    {
        public static UserResponse FromUser(User user)
        {
            return new UserResponse()
            {
                Id = user.Id,
                BusinessId = user.BusinessId,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                HasLeft = user.HasLeft
            };
        }
        public static User FromSignUpRequest(SignUpRequest request)
        {
            return new User()
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                Role = request.Role,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 13),
                BusinessId = request.BusinessId,
                HasLeft = false
            };
        }
    }
}
