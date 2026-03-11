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
        Task RegisterAsync(RegisterRequest req);
    }
}
