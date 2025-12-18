using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using UpTulse.Application.Helpers;
using UpTulse.Application.Models;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Identity;

namespace UpTulse.Application.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> ChangePasswordAsync(Guid userId, ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager
                .FindByIdAsync(userId.ToString()) ?? throw new DbRecordNotFoundException("User does not exist anymore");

            var changePasswordResult = await _userManager
                .ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), changePasswordRequest.NewPassword);

            var badRequestException = changePasswordResult.Errors.FirstOrDefault()?.Description;

            if (!changePasswordResult.Succeeded)
            {
                throw new DbRecordUpdateException($"Can`t update user password. Error: {badRequestException}");
            }

            var reAuth = await LoginAsync(new()
            {
                Username = user.UserName ?? string.Empty,
                Password = changePasswordRequest.NewPassword
            });

            return new LoginResponse
            {
                Email = user.Email ?? string.Empty,
                Fullname = user.FullName,
                Username = user.UserName ?? string.Empty,
                UserType = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty,
                Token = reAuth.Token ?? string.Empty
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginUserModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginUserModel.Username);

            if (user is null)
            {
                return new();
            }

            var signInResult = await _signInManager
                .PasswordSignInAsync(user, loginUserModel.Password, false, false);

            if (!signInResult.Succeeded)
            {
                return new();
            }

            return new LoginResponse
            {
                Username = user.UserName ?? string.Empty,
                Token = JwtHelper.GenerateToken(user, _configuration),
                UserType = user.Role,
                Fullname = user.FullName
            };
        }
    }
}