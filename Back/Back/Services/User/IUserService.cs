using Back.DTO.Login;
using Back.DTO.Register;

namespace Back.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user in the system. This method checks if a user with the provided email already exists. If it does, an exception is thrown. If not, a new user is created with the provided information (username, email, password, role) and stored in the database.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<string> RegisterAsync(RegisterRequest req);

        /// <summary>
        /// Tries to login a user. This method checks if a user with the provided email exists. If not, an exception is thrown. If the user exists, the provided password is verified against the stored hashed password. If the password is incorrect, an exception is thrown. If the login is successful, a JWT token is generated and returned.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<string> LoginAsync(LoginRequest req);
    }
}