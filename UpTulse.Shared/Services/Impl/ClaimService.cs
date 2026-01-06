using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace UpTulse.Shared.Services.Impl
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClaim(string key)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value ?? throw new KeyNotFoundException("Claim is NULL");
        }

        public string GetUserId()
        {
            try
            {
                return GetClaim(ClaimTypes.NameIdentifier);
            }
            catch
            {
                return Guid.Empty.ToString();
            }
        }

        public string GetUserRole()
        {
            return GetClaim(ClaimTypes.Role);
        }
    }
}