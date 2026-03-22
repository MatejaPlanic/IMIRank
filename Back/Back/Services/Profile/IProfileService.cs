using Back.DTO.Profile;

namespace Back.Services.Profile
{
    public interface IProfileService
    {
        /// <summary>
        /// Retrieves the profile information of a user based on their unique identifier (userId). The returned ProfileResponse object contains details such as the user's username, email, role, profile picture (if available), and the total number of reviews they have made. If no user is found with the given userId, an exception will be thrown indicating that the user was not found.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ProfileResponse> GetProfileAsync(string userId);

        /// <summary>
        /// Updates the username of a user identified by their unique identifier (userId). The new username is provided in the UpdateUsernameRequest object. This method will validate that the new username is not empty or whitespace before updating it in the database. If the validation fails, an exception will be thrown indicating that the username cannot be empty. If the user is not found with the given userId, an exception will be thrown indicating that the user was not found.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        Task UpdateUsernameAsync(string userId, UpdateUsernameRequest req);

        /// <summary>
        /// Updates the password of a user identified by their unique identifier (userId). The method takes an UpdatePasswordRequest object that contains the old password, new password, and confirmation of the new password. The method will first validate that the new password and confirmation match. If they do not match, an exception will be thrown indicating that the passwords do not match. Then, it will retrieve the user from the database using the userId. If the user is not found, an exception will be thrown indicating that the user was not found. Next, it will verify that the old password provided matches the user's current password using BCrypt. If the old password is incorrect, an exception will be thrown indicating that the old password is not correct. Finally, if all validations pass, the method will hash the new password and update it in the database for the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        Task UpdatePasswordAsync(string userId, UpdatePasswordRequest req);
    }
}
