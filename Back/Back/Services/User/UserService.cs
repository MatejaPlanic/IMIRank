using Back.DTO.Login;
using Back.DTO.Register;
using Back.Helpers;
using Back.Repositories.User;

namespace Back.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repo,IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginRequest req)
        {
            var exists = await _repo.FindByEmailAsync(req.Email);
            if (exists == null) throw new Exception("Korisnik sa zadatim Email-om ne postoji");

            if(BCrypt.Net.BCrypt.Verify(req.Password, exists.Password))
            {
                var key = _configuration["Jwt:Key"]!;
                return JwtProviderHelper.GenerateJwtToken(exists, key);
            }
            else
            {
                throw new Exception("Pogrešna lozinka");
            }
        }

        public async Task<string> RegisterAsync(RegisterRequest req)
        {
            var exists = await _repo.FindByEmailAsync(req.Email);
            if (exists != null) throw new Exception("Email već postoji");

            var user = new Models.User.User
            {
                UserName = req.UserName,
                Email = req.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = req.Role
            };
            await _repo.CreateAsync(user);

            var key = _configuration["Jwt:Key"] !;
            return JwtProviderHelper.GenerateJwtToken(user, key);
        }
    }
}