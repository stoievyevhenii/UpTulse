using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> ChangePasswordAsync(Guid userId, ChangePasswordRequest changePasswordRequest);

        Task<LoginResponse> LoginAsync(LoginRequest loginUserModel);
    }
}