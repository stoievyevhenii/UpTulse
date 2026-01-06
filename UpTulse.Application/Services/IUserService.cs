using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IUserService
    {
        Task<UserResponse> CreateAsync(CreateUserRequest createUserModel);

        Task<UserResponse> DisableUserAsync(Guid userId);

        Task<IEnumerable<UserResponse>> GetAllAsync();

        Task<UserResponse> GetUserAsync(Guid userId);

        Task<UserResponse> RecoveryUserAsync(Guid guid);

        Task<UserResponse> UpdateAsync(Guid userId, UpdateUserRequest updateUserRequest);
    }
}