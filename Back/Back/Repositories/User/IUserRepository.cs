namespace Back.Repositories.User
{
    public interface IUserRepository
    {
        /// <summary>
        /// Finds a user by their email address. Returns null if no user is found with the given email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Back.Models.User.User?> FindByEmailAsync(string email);

        /// <summary>
        /// Creates a new user in the database. The user object should contain all necessary information (e.g., username, email, password, role) before calling this method.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateAsync(Back.Models.User.User user);
    }
}
