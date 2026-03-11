using Back.DTO.Register;
using Back.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers.Auth
{
    [ApiController]
    [Route("auth/Register")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            try
            {
                await _userService.RegisterAsync(req);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
