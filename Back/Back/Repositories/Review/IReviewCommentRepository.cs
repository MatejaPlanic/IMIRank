using Back.Models.Review;

namespace Back.Repositories.Review
{
    public interface IReviewCommentRepository
    {
        /// <summary>
        /// Creates a new comment for a review. This method takes a ReviewComment object as input, which contains the details of the comment to be created, such as the reviewId it belongs to, the userId and userName of the commenter, and the content of the comment. The method will save the new comment to the database and return the created ReviewComment object with its assigned unique identifier (id) and timestamps. If the review associated with the comment does not exist, an exception will be thrown indicating that the review was not found.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<ReviewComment> CreateAsync(ReviewComment comment);

        /// <summary>
        /// Gets a comment by its unique identifier (id). This method retrieves a ReviewComment object from the database based on the provided id. If a comment with the specified id exists, it returns the ReviewComment object containing the details of the comment, such as the reviewId it belongs to, the userId and userName of the commenter, the content of the comment, and timestamps. If no comment is found with the given id, the method returns null, allowing the caller to handle the case where the comment does not exist without throwing an exception.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ReviewComment?> GetByIdAsync(string id);

        /// <summary>
        /// Gets a paginated list of comments for a specific review identified by its unique identifier (reviewId). This method retrieves a list of ReviewComment objects from the database that belong to the specified reviewId. The results are paginated based on the page and pageSize parameters, allowing efficient retrieval of comments in batches. If the review with the given reviewId does not exist, an exception will be thrown indicating that the review was not found. The method returns a list of ReviewComment objects for the specified page, or an empty list if no comments are found for that reviewId. Additionally, there is a separate method to count the total number of comments for a given reviewId, which can be used for pagination purposes.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<List<ReviewComment>> GetByReviewIdAsync(string reviewId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Counts the total number of comments for a specific review identified by its unique identifier (reviewId). This method is useful for pagination purposes when retrieving comments for a review, as it allows you to determine the total number of comments available for that reviewId. The method returns an integer representing the count of comments that belong to the specified reviewId. If the review with the given reviewId does not exist, an exception will be thrown indicating that the review was not found.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        Task<int> GetCommentCountByReviewIdAsync(string reviewId);

        /// <summary>
        /// Updates an existing comment identified by its unique identifier (id). This method allows updating the content of a comment. It takes the id of the comment to be updated and a ReviewComment object containing the new content for the comment. The method will check if the comment exists in the database. If it does, it will update the comment's content and timestamps accordingly. The method returns a boolean indicating whether the update was successful (true if the comment was found and updated, false otherwise). If no comment is found with the given id, it will return false, allowing the caller to handle the case where the comment does not exist without throwing an exception.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string id, ReviewComment comment);

        /// <summary>
        /// Deletes a comment identified by its unique identifier (id). This method allows deleting a comment from the database. It takes the id of the comment to be deleted and checks if the comment exists. If it does, it will remove the comment from the database and return true, indicating that the deletion was successful. If no comment is found with the given id, it will return false, allowing the caller to handle the case where the comment does not exist without throwing an exception.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);
    }
}
