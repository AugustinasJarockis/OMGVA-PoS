using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.Exceptions;
using OmgvaPOS.UserManagement.Enums;

namespace OmgvaPOS.HelperUtils;

public static class JwtTokenHandler
{
    public static TokenDetailsDTO GetTokenDetails(string? tokenString)
    {
        if (string.IsNullOrEmpty(tokenString))
            throw new BadRequestException("Authorization token must be provided");
            
        var jwtToken = GetJwtToken(tokenString);
            
        TokenDetailsDTO tokenDetails = new()
        {
            UserId = GetUserId(jwtToken),
            BusinessId = GetBusinessId(jwtToken),
            UserName = GetName(jwtToken),
            UserRole = GetUserRole(jwtToken) 
        };
            
        return tokenDetails;
    }
    
    public static long GetTokenBusinessId(string? tokenString) => GetTokenDetails(tokenString).BusinessId;

    private static long GetBusinessId(JwtSecurityToken token)
    {
        var businessIdString = token.GetClaimValue(ClaimTypes.Sid);
        return long.TryParse(businessIdString, out var businessId) 
            ? businessId 
            : throw new BadRequestException("Invalid business ID in token");
    }
        
    private static long GetUserId(JwtSecurityToken token)
    {
        var userIdString = token.GetClaimValue(ClaimTypes.NameIdentifier);
        return long.TryParse(userIdString, out var userId) 
            ? userId 
            : throw new BadRequestException("Invalid user ID in token");
    }
        
    private static string GetName(JwtSecurityToken token)
    {
        var userName = token.GetClaimValue(ClaimTypes.Name);
        return userName ?? throw new BadRequestException("Invalid user name in token");
    }
        
    private static UserRole GetUserRole(this JwtSecurityToken token)
    {
        var roleString = token.GetClaimValue(ClaimTypes.Role);
        return Enum.TryParse<UserRole>(roleString, true, out var role)
            ? role
            : throw new BadRequestException("Invalid role value in token");
    }
    
    private static JwtSecurityToken GetJwtToken(string authorizationString)
    {
        string token;
        try
        {
            token = authorizationString.Split(' ')[1];
        }
        catch (IndexOutOfRangeException)
        {
            throw new BadRequestException("Invalid authorization token format. Expected 'Bearer <token>'");
        }
            
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!jwtHandler.CanReadToken(token))
            throw new BadRequestException("Invalid token format");

        return jwtHandler.ReadJwtToken(token);
    }
    
    private static string? GetClaimValue(this JwtSecurityToken token, string claimType)
    {
        return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }
    
}