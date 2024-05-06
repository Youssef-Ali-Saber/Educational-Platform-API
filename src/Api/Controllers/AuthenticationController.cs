using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(
        IAuthenticationsService authenticationsService) : ControllerBase
    {
        [HttpPost("RegisterAsInstructor")]
        public async Task<IActionResult> RegisterAsInstructor(RegisterDto registerDto)
        {
            return Ok(await authenticationsService.Register(registerDto, "Instructor"));
        }
        [HttpPost("RegisterAsStudent")]
        public async Task<IActionResult> RegisterAsStudent(RegisterDto registerDto)
        {
            return Ok(await authenticationsService.Register(registerDto, "Student"));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await authenticationsService.Login(loginDto);
            if (result.Count == 2)
                return Ok(new
                {
                    token= result[0],
                    role= result[1]
                }
                );
            else if (result.Count==3)
                return StatusCode(500 , new
                {
                    massege = result[1],
                    errors = result[2]
                }
                );
            return BadRequest(result);

        }
      
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword,string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await authenticationsService.ChangePassword(userId, oldPassword, newPassword);
            return Ok();
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var s =  await authenticationsService.ForgotPassword(email);
            return Ok(s);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email,string token,string newPassword)
        {
            var s = await authenticationsService.ResetPassword(email, token, newPassword);
            return Ok(s);
        }

    }
}
