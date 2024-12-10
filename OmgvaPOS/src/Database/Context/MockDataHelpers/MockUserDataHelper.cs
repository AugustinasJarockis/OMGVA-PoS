using Microsoft.EntityFrameworkCore;
using OmgvaPOS.UserManagement.Enums;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers;

public static class MockUserDataHelper
{

    public static async Task InitializeMockUsersAsync(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Adding mock users...");
        await dbContext.Users.AddRangeAsync(MockUsers());
        await dbContext.SaveChangesAsync();
        logger.LogDebug("Mock users added.");
    }
    
    public static async Task RemoveAllUsersAsync(OmgvaDbContext dbContext, ILogger logger)
    {
        logger.LogDebug("Removing all users...");
        var allUsers = await dbContext.Users.ToListAsync();
        dbContext.Users.RemoveRange(allUsers);
        await dbContext.SaveChangesAsync();
        logger.LogDebug("All users removed.");
    }

    private static IEnumerable<User> MockUsers()
    {
        return new List<User>
        {
            // Users for OMGVA
            MockUser("Owner Omgva", "ownerOmgva", "owner@omgva.com",
                     UserRole.Owner, "ownerPass", MockBusinessesDataHelper.OmgvaBusinessId),
            MockUser("Admin Omgva", "adminOmgva", "admin@omgva.com",
                     UserRole.Admin, "adminPass", MockBusinessesDataHelper.OmgvaBusinessId),
            MockUser("Employee Omgva", "employeeOmgva", "employee@omgva.com",
                     UserRole.Employee, "employeePass", MockBusinessesDataHelper.OmgvaBusinessId),

            // Users for Different business
            MockUser("Owner DiffBusiness", "ownerDiffBusiness", "owner@diffbusiness.com",
                UserRole.Owner, "ownerPass", MockBusinessesDataHelper.DiffBusinessId),
            MockUser("Admin DiffBusiness", "adminDiffBusiness", "admin@diffbusiness.com",
                UserRole.Admin, "adminPass", MockBusinessesDataHelper.DiffBusinessId),
            MockUser("Employee DiffBusiness", "adminDiffBusiness", "employee@diffbusiness.com",
                UserRole.Employee, "employeePass", MockBusinessesDataHelper.DiffBusinessId),
            
            // Users without a business
            MockUser("Owner NoBusiness", "ownerNoBusiness", "owner@nobusiness.com",
                     UserRole.Owner, "ownerPass"),
            MockUser("Admin NoBusiness", "adminNoBusiness", "admin@nobusiness.com",
                     UserRole.Admin, "adminPass"),
            MockUser("Employee NoBusiness", "adminNoBusiness", "employee@nobusiness.com",
                     UserRole.Employee, "employeePass")
        };
    }

    private static User MockUser(
        string name,
        string username,
        string email,
        UserRole role,
        string password,
        long? businessId = null,
        bool hasLeft = false)
    {
        return new User
        {
            BusinessId = businessId,
            Name = name,
            Username = username,
            Email = email,
            Role = role,
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13),
            HasLeft = hasLeft
        };
    }
}