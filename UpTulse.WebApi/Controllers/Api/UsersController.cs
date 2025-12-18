using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Models;

namespace UpTulse.WebApi.Controllers.Api
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserRequest createUserRequest)
        {
            return Ok(ApiResult<UserResponse>.Success(await _userService.CreateAsync(createUserRequest)));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            return Ok(ApiResult<UserResponse>.Success(await _userService.DisableUserAsync(id)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(ApiResult<IEnumerable<UserResponse>>.Success(await _userService.GetAllAsync()));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(ApiResult<UserResponse>.Success(await _userService.GetUserAsync(id)));
        }

        [HttpPut("{id:guid}/recovery")]
        public async Task<IActionResult> RecoveryAsync(Guid id)
        {
            return Ok(ApiResult<UserResponse>.Success(await _userService.RecoveryUserAsync(id)));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserRequest updateUserRequest)
        {
            return Ok(ApiResult<UserResponse>.Success(await _userService.UpdateAsync(id, updateUserRequest)));
        }
    }
}