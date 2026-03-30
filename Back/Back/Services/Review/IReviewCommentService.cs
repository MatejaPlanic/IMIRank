using Back.DTO.Review;
using Back.Models.Review;

namespace Back.Services.Review
{
    public interface IReviewCommentService
    {
        /// <summary>
        /// Creates a new comment for a review. This method takes the reviewId to which the comment belongs, the userId and userName of the commenter, and a CreateReviewCommentRequest object that contains the content of the comment. The method will create a new ReviewComment object, save it to the database, and return a ReviewCommentResponse object containing the details of the created comment. If the review with the given reviewId does not exist, an exception will be thrown indicating that the review was not found.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ReviewCommentResponse> CreateCommentAsync(string reviewId, string userId, string userName, CreateReviewCommentRequest request);

        /// <summary>
        /// Retrieves a comment by its unique identifier (id). This method returns a ReviewCommentResponse object containing the details of the comment, such as the comment content, author information, and timestamps. If no comment is found with the given id, the method returns null. This allows the caller to handle the case where the comment does not exist without throwing an exception.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ReviewCommentResponse?> GetCommentByIdAsync(string id);

        /// <summary>
        /// Retrieves a paginated list of comments for a specific review identified by its unique identifier (reviewId). The method takes the reviewId to filter comments belonging to that review, and supports pagination through the page and pageSize parameters. It returns a ReviewCommentsListResponse object that contains a list of ReviewCommentResponse objects for the specified page, along with pagination metadata such as total count and total pages. If no review is found with the given reviewId, an exception will be thrown indicating that the review was not found.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<ReviewCommentsListResponse> GetCommentsByReviewIdAsync(string reviewId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Updates an existing comment identified by its unique identifier (id). This method allows a user to update the content of their comment. It takes the id of the comment to be updated, the userId of the commenter to verify ownership, and an UpdateReviewCommentRequest object that contains the new content for the comment. The method will check if the comment exists and if the userId matches the author of the comment. If both checks pass, it will update the comment's content in the database and return true. If the comment does not exist or if the userId does not match the author, it will return false, indicating that the update was not successful.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> UpdateCommentAsync(string id, string userId, UpdateReviewCommentRequest request);

        /// <summary>
        /// Deletes a comment identified by its unique identifier (id). This method allows a user to delete their comment. It takes the id of the comment to be deleted and the userId of the commenter to verify ownership. The method will check if the comment exists and if the userId matches the author of the comment. If both checks pass, it will delete the comment from the database and return true. If the comment does not exist or if the userId does not match the author, it will return false, indicating that the deletion was not successful.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentAsync(string id, string userId);
    }
}
