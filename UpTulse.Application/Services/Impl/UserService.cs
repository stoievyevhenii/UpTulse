using FluentValidation;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UpTulse.Application.Models;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Identity;
using UpTulse.Shared.Constants;

namespace UpTulse.Application.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IValidator<CreateUserRequest> createUserValidator)
        {
            _createUserValidator = createUserValidator;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest createUserModel)
        {
            var validationResult = await _createUserValidator.ValidateAsync(createUserModel);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid user data");
            }

            var recordAlreadyExists = await _userManager.FindByNameAsync(createUserModel.Username);

            if (recordAlreadyExists != null)
            {
                throw new BadRequestException("User with the same username already exists");
            }

            if (!await _roleManager.RoleExistsAsync(createUserModel.Role))
            {
                throw new DbRecordNotFoundException("Role does not exist");
            }

            var user = new ApplicationUser
            {
                UserName = createUserModel.Username,
                FullName = createUserModel.FullName,
                Role = createUserModel.Role
            };

            var result = await _userManager.CreateAsync(user, createUserModel.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to create user");
            }

            return new UserResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        public async Task<UserResponse> DisableUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new BadRequestException("User does not exist anymore");

            if (user.Role == UserRole.Admin)
            {
                var activeCount = await _userManager.Users.CountAsync(f => f.LockoutEnd == null && f.Role == UserRole.Admin);

                if (activeCount == 1)
                {
                    throw new BadRequestException("You can't disable the last user with this role");
                }
            }

            var disableUserTask = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            if (!disableUserTask.Succeeded)
            {
                throw new BadRequestException(disableUserTask.Errors.First().Description);
            }

            await _signInManager.SignOutAsync();

            return new UserResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName ?? string.Empty,
                FullName = user.FullName,
                Role = user.Role,
                LockoutEnd = user.LockoutEnd
            };
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var usersList = _userManager.Users.Select(user => new UserResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName ?? string.Empty,
                FullName = user.FullName,
                Role = user.Role,
                LockoutEnd = user.LockoutEnd
            });

            return await usersList.ToListAsync();
        }

        public async Task<UserResponse> GetUserAsync(Guid userId)
        {
            var result = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new BadRequestException("User does not exist anymore");

            return new UserResponse
            {
                Id = Guid.Parse(result.Id),
                Username = result.UserName ?? string.Empty,
                FullName = result.FullName,
                Role = result.Role,
                LockoutEnd = result.LockoutEnd
            };
        }

        public async Task<UserResponse> RecoveryUserAsync(Guid guid)
        {
            var user = await _userManager.FindByIdAsync(guid.ToString())
                ?? throw new BadRequestException("User does not exist anymore");

            var recoveryUserTask = await _userManager.SetLockoutEndDateAsync(user, null);

            if (!recoveryUserTask.Succeeded)
            {
                throw new BadRequestException("Failed to recover user");
            }

            return new UserResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName ?? string.Empty,
                FullName = user.FullName,
                Role = user.Role,
                LockoutEnd = user.LockoutEnd
            };
        }

        public async Task<UserResponse> UpdateAsync(Guid userId, UpdateUserRequest updateUserRequest)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new BadRequestException("User does not exist anymore");

            var oldRole = user.Role;

            if (oldRole == UserRole.Admin)
            {
                var activeCount = await _userManager.Users.CountAsync(f => f.LockoutEnd == null && f.Role == UserRole.Admin);

                if (activeCount == 1)
                {
                    throw new BadRequestException("You can't disable the last user with this role");
                }
            }

            user.FullName = updateUserRequest.FullName.Trim();
            user.UserName = updateUserRequest.Username.Trim().ToLower();
            user.Role = updateUserRequest.Role.Trim();

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new BadRequestException("Failed to update user");
            }

            if (oldRole != updateUserRequest.Role)
            {
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, updateUserRequest.Role);
            }

            return new UserResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName ?? string.Empty,
                FullName = user.FullName,
                Role = user.Role,
                LockoutEnd = user.LockoutEnd
            };
        }
    }
}