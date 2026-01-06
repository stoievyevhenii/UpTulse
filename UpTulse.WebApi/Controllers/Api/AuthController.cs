using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Shared.Models;

namespace UpTulse.WebApi.Controllers.Api
{
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginRequest loginUserModel)
        {
            return Ok(ApiResult<LoginResponse>.Success(await _authService.LoginAsync(loginUserModel)));
        }
    }
}