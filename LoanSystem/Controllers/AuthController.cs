using LoanSystem.API.Common;
using LoanSystem.Application.DTOs.Auth;
using LoanSystem.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LoanSystem.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : BaseController
    {
        [HttpPost("register")]
        public async Task<ActionResult<TokenResponseDto>> Register([FromBody] RegisterDto request)
        {
            var result = await authService.Register(request);
            
            if (result == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto request)
        {
            var result = await authService.Login(request);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }
    }
}
