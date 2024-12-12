using Microsoft.IdentityModel.Tokens;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.AuthManagement.Repository;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OmgvaPOS.AuthManagement.Service
{
    public class AuthService(IAuthenticationRepository authenticationRepository, IConfiguration config) : IAuthService
    {
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        private readonly IConfiguration _config = config;

        public User SignUp(SignUpRequest signUpRequest)
        {
            try
            {
                if (signUpRequest == null)
                    throw new ArgumentNullException(nameof(signUpRequest));

                User user = new()
                {
                    Name = signUpRequest.Name,
                    Username = signUpRequest.Username,
                    Email = signUpRequest.Email,
                    Role = signUpRequest.Role,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(signUpRequest.Password, 13),
                    BusinessId = signUpRequest.BusinessId,
                    HasLeft = false
                };

                return _authenticationRepository.SignUpUser(user); 
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during sign-in.", ex);
            }
        }

        public bool IsSignedUp(string username, string password)
        {
            return _authenticationRepository.AnyUserDuplicate(username, password);
        }

        public bool IsEmailUsed(string email)
        {
            return _authenticationRepository.AnyUserEmailDuplicate(email);
        }

        public bool IsUsernameUsed(string username)
        {
            return _authenticationRepository.AnyUserUsernameDuplicate(username);
        }
        public LoginDTO Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    throw new ArgumentNullException(nameof(loginRequest));

                var user = _authenticationRepository.GetUserByUsername(loginRequest.Username);

                if (user.HasLeft)
                    return new LoginDTO(false, "This account is deactivated");

                bool checkPassword = BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, user.Password);

                if (checkPassword)
                {
                    UserResponse userResponse = new()
                    {
                        Id = user.Id,
                        BusinessId = user.BusinessId,
                        Name = user.Name,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role,
                        HasLeft = user.HasLeft
                    };
                    return new LoginDTO(true, "Login Successfully", GenerateJWT(userResponse));
                }
                return new LoginDTO(false, "Invalid Password");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during login.", ex);
            }
        }
        public LoginDTO GenerateAdminJwtToken(long businessId, TokenDetailsDTO tokenDetails)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, tokenDetails.NameIdentifier),
                 new Claim(ClaimTypes.Role, tokenDetails.Role),
                 new Claim(ClaimTypes.Name, tokenDetails.Name),
                 new Claim(ClaimTypes.Sid, businessId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new LoginDTO(true, "Login Successfully", new JwtSecurityTokenHandler().WriteToken(token));
        }
        private string GenerateJWT(UserResponse user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Role, user.Role.ToString()!),
                 new Claim(ClaimTypes.Name, user.Name!),
                 new Claim(ClaimTypes.Sid, user.BusinessId.ToString()!)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
