using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.HelperUtils
{
    public static class AuthorizationHandler
    {
        public static bool CanManageBusiness(string? tokenString, long businessId)
        {
            var token = JwtTokenHandler.GetTokenDetails(tokenString);
            
            if (token.BusinessId != businessId)
                return false;

            if (token.UserRole == UserRole.Admin 
                || token.UserRole == UserRole.Owner 
                || token.UserRole == UserRole.Employee)
            {
                return true;
            }
            
            return false;
        }

        public static bool CanManageUser(string? tokenString, long businessId, long userId)
        {
            var token = JwtTokenHandler.GetTokenDetails(tokenString);

            if (token.UserRole == UserRole.Admin)
                return true;

            if (token.UserRole == UserRole.Employee && token.UserId == userId)
                return true;

            if (token.UserRole == UserRole.Owner && token.BusinessId == businessId)
                return true;
            
            return false;
        }

        public static bool CanDeleteUser(string? tokenString, long businessId, long userId)
        {
            var token = JwtTokenHandler.GetTokenDetails(tokenString);

            if (token.UserRole == UserRole.Admin)
                return true;

            if (token.UserRole == UserRole.Owner && token.BusinessId == businessId)
                return true;
            
            if (token.UserRole == UserRole.Employee && token.UserId == userId)
                return true;
            
            // previously.
            // TODO: Not sure what '(token.UserRoleEquals(UserRole.Owner) || token.UserRoleEquals(UserRole.Admin)) && token.UserIdEquals(userId)' is for
            // var token = GetJwtToken(tokenString);
            // if (token == null
            //     || token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(businessId)
            //     || (token.UserRoleEquals(UserRole.Owner) || token.UserRoleEquals(UserRole.Admin)) && token.UserIdEquals(userId)
            //    )
            //     return false;
            // return true;

            return false;
        }
        
    }
}