using Microsoft.IdentityModel.Tokens;
using OmgvaPOS.AuthManagement.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OmgvaPOS.AuthManagement.Validators;
using OmgvaPOS.UserManagement.Repository;

namespace OmgvaPOS.AuthManagement.Service
{
    public class AuthService(IUserRepository userRepository, IConfiguration config) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _config = config;
        
        private const string UsernameOrPasswordIncorrect = "Username or password is incorrect";
        private const string AccountDeactivated = "This account is deactivated";
        private const string LoginSuccessful = "Login Successful";
        private const string DefaultBusinessId = "-1";

        public LoginResponseDTO Login(LoginRequest loginRequest)
        {
            AuthValidator.ValidateLoginRequest(loginRequest);
            
            var user = _userRepository.GetUserByUsername(loginRequest.Username);
            if (user == null)
                return new LoginResponseDTO(false, UsernameOrPasswordIncorrect);

            if (user.HasLeft)
                return new LoginResponseDTO(false, AccountDeactivated);

            var passwordIsCorrect = BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, user.Password);
            if (!passwordIsCorrect)
                return new LoginResponseDTO(false, UsernameOrPasswordIncorrect);

            var token = GenerateJWT(user.Id.ToString(), user.Role.ToString(), 
                                         user.Name, user.BusinessId?.ToString() ?? DefaultBusinessId);
            return new LoginResponseDTO(true, LoginSuccessful, token);
        }
        
        public LoginResponseDTO LoginAdminWithDifferentBusiness(long newBusinessId, TokenDetailsDTO tokenDetails)
        {
            var newToken = GenerateJWT(
                userId: tokenDetails.UserId.ToString(),
                role: tokenDetails.UserRole.ToString(),
                name: tokenDetails.UserName,
                businessId: newBusinessId.ToString());

            return new LoginResponseDTO(true, LoginSuccessful, newToken);
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
                    var userResponse = UserMapper.FromUser(user);
                    return new LoginDTO(true, "Login Successfully", GenerateJWT(userResponse));
                }
                return new LoginDTO(false, "Invalid Password");
            }
            catch (KeyNotFoundException) {
                return new LoginDTO(false, "No user with that username exists.");
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
                 new Claim(ClaimTypes.NameIdentifier, userId),
                 new Claim(ClaimTypes.Role, role),
                 new Claim(ClaimTypes.Name, name),
                 new Claim(ClaimTypes.Sid, businessId)
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
