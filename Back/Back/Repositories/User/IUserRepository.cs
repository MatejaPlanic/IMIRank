using Back.Models.Enums;

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

        /// <summary>
        /// Counts the total number of users in the database.
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        /// <summary>
        /// Counts the number of users in the database that have a specific role (e.g., Admin, User). 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<long> CountByRoleAsync(UserRole role);
    }
}