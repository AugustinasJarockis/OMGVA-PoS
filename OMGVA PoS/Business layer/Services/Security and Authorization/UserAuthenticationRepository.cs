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
    public class UserAuthenticationRepository(OMGVADbContext database, IUserRepository userRepository, IConfiguration config) : IUserAuthenticationRepository
    {
        private readonly OMGVADbContext _database = database;
        private readonly IUserRepository _userRepository= userRepository;
        private readonly IConfiguration _config = config;
        public User SignIn(SignInModel signInModel)
        {
            User user = new()
            {
                Name = signInModel.Name,
                Username = signInModel.Username,
                Email = signInModel.Email,
                Role = signInModel.Role,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(signInModel.Password, 13),
                BusinessId = signInModel.BusinessId,
                HasLeft = false
            };

            _database.Users.Add(user);
            _database.SaveChanges();

            return _userRepository.GetUser(_userRepository.GetUserId(user.Username));
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

        public async Task<LoginDTO> Login (LoginModel loginModel)
        {
            var getUserId = _userRepository.GetUserId(loginModel.Username);
            var getUser = _userRepository.GetUser(getUserId);

            if (getUser.HasLeft)
                return new LoginDTO(false, "This account is deactivated");

            bool checkPassword = BCrypt.Net.BCrypt.EnhancedVerify(loginModel.Password, getUser.Password);
            if (checkPassword)

                return new LoginDTO(true, "Login Successfully", GeneretateJWT(getUser));
            else
                return new LoginDTO(false, "Invalid Password");
        }
        private string GeneretateJWT(User user)
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
