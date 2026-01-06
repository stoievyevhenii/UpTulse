using Microsoft.AspNetCore.Identity;

using UpTulse.DataAccess.Identity;
using UpTulse.Shared.Constants;

namespace UpTulse.DataAccess.Persistence
{
    public static class DatabaseContextSeed
    {
        private const string DefaultAdminUserName = "admin";

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(roleName: UserRole.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            }

            if (!await roleManager.RoleExistsAsync(UserRole.User))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));
            }
        }

        public static async Task SeedUsersAsync(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            var defaultAdminRecord = await userManager.FindByNameAsync(DefaultAdminUserName);

            if (defaultAdminRecord is null)
            {
                var user = new ApplicationUser
                {
                    UserName = DefaultAdminUserName,
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    Role = UserRole.Admin,
                    FullName = "Standart Administrator"
                };

                await userManager.CreateAsync(user, "Admin123.?");
                await context.SaveChangesAsync();
            }
        }
    }
}