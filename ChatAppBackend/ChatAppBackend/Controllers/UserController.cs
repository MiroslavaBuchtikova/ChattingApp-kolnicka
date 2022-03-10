using ChatAppBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid credentials");
            var token = await _userService.GetToken(login);
            return string.IsNullOrEmpty(token) ? BadRequest("Invalid email or password") : Ok(token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Errors in registration data");
            var success = await _userService.RegisterUser(model);

            return success ? Ok("Registration successful") : BadRequest("Registration failed");
        }
    }
}
