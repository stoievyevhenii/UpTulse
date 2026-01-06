using Microsoft.AspNetCore.Identity;

using UpTulse.Shared.Constants;

namespace UpTulse.DataAccess.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = Guid.CreateVersion7().ToString();
        public string Role { get; set; } = UserRole.User;
    }
}