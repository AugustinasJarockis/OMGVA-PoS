using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.HelperUtils
{
    public static class JwtTokenHandler
    {
        private static readonly string ClaimTypeNameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private static readonly string ClaimTypeSid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";
        private static readonly string ClaimTypeName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        private static readonly string ClaimTypeRole = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        private static JwtSecurityToken GetJwtToken(string tokenString)
        {
            if (tokenString != null)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                tokenString = tokenString.Split(' ')[1];
                bool isReadable = jwtHandler.CanReadToken(tokenString);
                if (isReadable)
                {
                    JwtSecurityToken token = jwtHandler.ReadJwtToken(tokenString);
                    return token;
                }
            }
            return null!;
        }

        public static bool CanManageBusiness(string tokenString, long businessId)
        {
            var token = GetJwtToken(tokenString);
            if (token == null
                || (token.UserRoleEquals(UserRole.Owner) || token.UserRoleEquals(UserRole.Employee))
                    && !token.UserBusinessEquals(businessId)
                )
                return false;
            return true;
        }

        public static bool CanManageUser(string tokenString, long businessId, long userId)
        {
            var token = GetJwtToken(tokenString);
            if (token == null
                || token.UserRoleEquals(UserRole.Employee) && !token.UserIdEquals(userId)
                || token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(businessId)
                )
                return false;
            return true;
        }

        public static bool CanDeleteUser(string tokenString, long businessId, long userId)
        {
            var token = GetJwtToken(tokenString);
            if (token == null
                || token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(businessId)
                || (token.UserRoleEquals(UserRole.Owner) || token.UserRoleEquals(UserRole.Admin)) && token.UserIdEquals(userId)
                )
                return false;
            return true;
        }

        public static long? GetTokenBusinessId(string tokenString)
        {
            var token = GetJwtToken(tokenString);
            try
            {
                return long.Parse(token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeSid))!.Value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static long? GetTokenUserId(string tokenString)
        {
            var token = GetJwtToken(tokenString);
            try
            {
                return long.Parse(token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeNameIdentifier))!.Value);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static TokenDetailsDTO GetTokenDetails(string tokenString)
        {
            var token = GetJwtToken(tokenString);
            TokenDetailsDTO tokenDetails = new()
            {
                Name = token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeName)).Value,
                NameIdentifier = token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeNameIdentifier)).Value,
                Role = token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeRole)).Value
            };
            return tokenDetails;
        }

        private static bool UserBusinessEquals(this JwtSecurityToken token, long businessId)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeSid))?.Value == businessId.ToString();
        }

        private static bool UserRoleEquals(this JwtSecurityToken token, UserRole role)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeRole))?.Value
                == role.ToString();
        }

        private static bool UserIdEquals(this JwtSecurityToken token, long userId)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeNameIdentifier))?.Value == userId.ToString();
        }

        //NOTE: Checks if name equals to the one in the token. Not if username is equal.
        private static bool UserNameEquals(this JwtSecurityToken token, string userName)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypeName))?.Value == userName;
        }
    }
}