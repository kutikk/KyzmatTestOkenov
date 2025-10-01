using Microsoft.AspNetCore.Mvc;
using okenovTest.Dto;
using okenovTest.Services;

namespace okenovTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase  
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Username, dto.Password);
            return token != null ? Ok(new { token }) : Unauthorized(); 
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string authHeader)
        {
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var token = authHeader.Substring("Bearer ".Length).Trim();
            await _authService.LogoutAsync(token);
            return Ok(new { message = "Logged out successfully" });
        }

    }
}
