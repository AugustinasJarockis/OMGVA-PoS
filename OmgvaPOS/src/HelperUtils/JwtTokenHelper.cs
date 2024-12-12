﻿using System.IdentityModel.Tokens.Jwt;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.HelperUtils
{
    public static class JwtTokenHelper {
        public static JwtSecurityToken GetJwtToken(string tokenString) {
            if (tokenString != null) {
                var jwtHandler = new JwtSecurityTokenHandler();
                tokenString = tokenString.Split(' ')[1];
                bool isReadable = jwtHandler.CanReadToken(tokenString);
                if (isReadable) {
                    JwtSecurityToken token = jwtHandler.ReadJwtToken(tokenString);
                    return token;
                }
            }
            return null;
        }

        public static bool UserBusinessEquals(this JwtSecurityToken token, long businessId) {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid")).Value == businessId.ToString();
        }

        public static bool UserRoleEquals(this JwtSecurityToken token, UserRole role) {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")).Value
                == (role switch {
                    UserRole.Employee => "Employee",
                    UserRole.Owner => "Owner",
                    UserRole.Admin => "Admin"
                });
        }

        public static bool UserIdEquals(this JwtSecurityToken token, long userId) {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value == userId.ToString();
        }

        //NOTE: Checks if name equals to the one in the token. Not if username is equal.
        public static bool UserNameEquals(this JwtSecurityToken token, string userName) {
            return token.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")).Value == userName;
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
    }
}
