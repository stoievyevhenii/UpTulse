using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> LoginAsync(LoginRequest loginUserModel)
        {
            return Ok(ApiResult<LoginResponse>.Success(await _authService.LoginAsync(loginUserModel)));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            return Ok(true);
        }
    }
}