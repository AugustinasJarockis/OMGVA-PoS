using System.IdentityModel.Tokens.Jwt;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.HelperUtils
{
    public static class JwtTokenHandler
    {
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
                return long.Parse(token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"))!.Value);
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
                return long.Parse(token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))!.Value);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static TokenDetailsDTO GetTokenDetails(this JwtSecurityToken token)
        {
            TokenDetailsDTO tokenDetails = new()
            {
                Name = token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")).Value,
                NameIdentifier = token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value,
                Role = token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")).Value
            };
            return tokenDetails;
        }

        private static bool UserBusinessEquals(this JwtSecurityToken token, long businessId)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"))?.Value == businessId.ToString();
        }

        private static bool UserRoleEquals(this JwtSecurityToken token, UserRole role)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))?.Value
                == role.ToString();
        }

        private static bool UserIdEquals(this JwtSecurityToken token, long userId)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value == userId.ToString();
        }

        //NOTE: Checks if name equals to the one in the token. Not if username is equal.
        private static bool UserNameEquals(this JwtSecurityToken token, string userName)
        {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))?.Value == userName;
        }
    }
}