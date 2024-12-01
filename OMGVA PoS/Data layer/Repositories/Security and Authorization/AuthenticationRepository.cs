using Microsoft.IdentityModel.Tokens;
using OMGVA_PoS.Business_layer.Services.UserManagement;
using OMGVA_PoS.Data_layer.Context;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Helper_modules.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OMGVA_PoS.Business_layer.Services.Security_and_Authorization
{
    public class AuthenticationRepository(OMGVADbContext database, IUserRepository userRepository, IConfiguration config) : IAuthenticationRepository
    {
        private readonly OMGVADbContext _database = database;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _config = config;

        public User SignIn(SignInRequest signInRequest)
        {
            try
            {
                if (signInRequest == null)
                    throw new ArgumentNullException(nameof(signInRequest));

                User user = new()
                {
                    Name = signInRequest.Name,
                    Username = signInRequest.Username,
                    Email = signInRequest.Email,
                    Role = signInRequest.Role,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(signInRequest.Password, 13),
                    BusinessId = signInRequest.BusinessId,
                    HasLeft = false
                };

                _database.Users.Add(user);
                _database.SaveChanges();

                return _userRepository.GetUser(_userRepository.GetUserId(user.Username));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during sign-in.", ex);
            }
        }

        public async Task<LoginDTO> Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    throw new ArgumentNullException(nameof(loginRequest));

                var getUserId = _userRepository.GetUserId(loginRequest.Username);
                var getUser = _userRepository.GetUser(getUserId);

                if (getUser.HasLeft)
                    return new LoginDTO(false, "This account is deactivated");

                bool checkPassword = BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, getUser.Password);
                return checkPassword
                    ? new LoginDTO(true, "Login Successfully", GenerateJWT(getUser))
                    : new LoginDTO(false, "Invalid Password");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during login.", ex);
            }
        }

        public bool IsSignedIn(string username, string password)
        {
            return _database.Users.Where(u => u.Username == username && u.Password == password).Any();
        }

        public bool IsEmailUsed(string email)
        {
            return _database.Users.Any(u => u.Email == email);
        }

        public bool IsUsernamelUsed(string username)
        {
            return _database.Users.Any(u => u.Username == username);
        }

        private string GenerateJWT(User user)
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
