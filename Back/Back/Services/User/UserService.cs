using Back.DTO.Register;
using Back.Repositories.User;

namespace Back.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task RegisterAsync(RegisterRequest req)
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
        }
    }
}
