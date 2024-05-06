using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivesController(IUserService userService) : ControllerBase
    {
        [HttpGet("GetChat/{courseId}")]
        [Authorize]
        public IActionResult GetChat(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chat = userService.GetChat(userId, courseId);   
            return Ok(chat);
        }
        [HttpGet("GetProfile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = userService.GetProfile(userId);
            return Ok(profile);
        }
        [HttpDelete("DeleteAccount")]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userService.DeleteAccount(userId);
            return Ok();
        }
        [HttpPut("UpdateProfile")]
        [Authorize]
        public IActionResult UpdateProfile(string? email, string? name, string? phone)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userService.UpdateProfile(userId, email, name, phone);
            return Ok();
        }

    }
}
