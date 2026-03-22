using Back.DTO.Profile;
using Back.Repositories.User;
using Back.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IUserRepository _userRepo;
        private readonly IWebHostEnvironment _env;

        public ProfileController(IProfileService profileService, IUserRepository userRepo, IWebHostEnvironment env)
        {
            _profileService = profileService;
            _userRepo = userRepo;
            _env = env;
        }

        private string UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User not authenticated");

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var profile = await _profileService.GetProfileAsync(UserId);
                return Ok(profile);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameRequest req)
        {
            try
            {
                await _profileService.UpdateUsernameAsync(UserId, req);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest req)
        {
            try
            {
                await _profileService.UpdatePasswordAsync(UserId, req);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("picture")]
        public async Task<IActionResult> UpdatePicture(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Fajl je prazan" });

                var fileName = $"{UserId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = Path.Combine("images", fileName).Replace("\\", "/");
                await _userRepo.UpdateProfilePictureAsync(UserId, relativePath);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}