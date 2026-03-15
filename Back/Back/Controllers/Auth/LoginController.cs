using Back.DTO.Login;
using Back.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var jwt = await _userService.LoginAsync(req);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}