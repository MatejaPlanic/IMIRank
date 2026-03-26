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

        /// <summary>
        /// Gets all users from the database.
        /// </summary>
        /// <returns></returns>
        Task<List<Models.User.User>> GetAllAsync();

        /// <summary>
        /// Retrieves a user from the database by their unique identifier (ID). Returns null if no user is found with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Models.User.User?> GetByIdAsync(string id);

        /// <summary>
        /// Updates the username of a user identified by their unique ID. The new username should be provided as a parameter. This method does not return any value, but it will update the user's username in the database if the user exists. If no user is found with the given ID, no changes will be made.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUsername"></param>
        /// <returns></returns>
        Task UpdateUsernameAsync(string id, string newUsername);

        /// <summary>
        /// Updates the password of a user identified by their unique ID. The new password should be provided as a parameter, and it is expected to be already hashed before being passed to this method. This method does not return any value, but it will update the user's password in the database if the user exists. If no user is found with the given ID, no changes will be made.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newHashedPassword"></param>
        /// <returns></returns>
        Task UpdatePasswordAsync(string id, string newHashedPassword);

        /// <summary>
        /// Updates the profile picture path of a user identified by their unique ID. The new profile picture path should be provided as a string (e.g., representing the file path). This method does not return any value, but it will update the user's profile picture path in the database if the user exists. If no user is found with the given ID, no changes will be made.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="picturePath"></param>
        /// <returns></returns>
        Task UpdateProfilePictureAsync(string id, string picturePath);

        /// <summary>
        /// Retrieves all users that match a username query.
        /// </summary>
        Task<List<Models.User.User>> SearchByUserNameAsync(string query, int page, int pageSize);

        /// <summary>
        /// Counts all users that match a username query.
        /// </summary>
        Task<long> CountByUserNameAsync(string query);

        /// <summary>
        /// Retrieves all users with the Editor role from the database.
        /// </summary>
        /// <returns></returns>
        Task<List<Models.User.User>> GetEditorsByRoleAsync();
    }
}