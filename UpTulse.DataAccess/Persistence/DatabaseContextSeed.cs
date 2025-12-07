using Microsoft.AspNetCore.Identity;

using UpTulse.DataAccess.Identity;
using UpTulse.Shared.Constants;

namespace UpTulse.DataAccess.Persistence
{
    public static class DatabaseContextSeed
    {
        public static async Task SeedUsersAsync(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            var defaultAdminRecord = await userManager.FindByNameAsync("admin");

            if (defaultAdminRecord is null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    Role = UserRole.Admin,
                    FullName = "Standart Administrator"
                };

                await userManager.CreateAsync(user, "admin");
                await context.SaveChangesAsync();
            }
        }
    }
}