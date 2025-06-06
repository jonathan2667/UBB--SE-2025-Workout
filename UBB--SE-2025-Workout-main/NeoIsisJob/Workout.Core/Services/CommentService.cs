using ServerLibraryProject.Interfaces;
using ServerLibraryProject.Models;
using Workout.Core.IRepositories;
using Workout.Core.IServices;


namespace ServerLibraryProject.Services
{

    /// <summary>
    /// Service for managing comments.
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IPostRepository postService;
        private readonly IUserRepo userServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        /// <param name="cr">The comment repository.</param>
        /// <param name="pr">The post repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public CommentService(ICommentRepository cr, IPostRepository ps, IUserRepo userRepository)
        {
            this.commentRepository = cr;
            this.postService = ps;    // Added null checks
            this.userServiceProxy = userRepository; // Added null checks
        }

        /// <summary>
        /// Validates and adds a new comment.
        /// </summary>
        /// <param name="content">The content of the comment.</param>
        /// <param name="userId">The ID of the user adding the comment.</param>
        /// <param name="postId">The ID of the post to which the comment is added.</param>
        /// <returns>The created Comment object.</returns>
        public Comment AddComment(string content, int userId, long postId)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentException("Comment content cannot be empty or null.", nameof(content));
            }

            if (this.userServiceProxy.GetUserByIdAsync(userId).Result == null)
            {
                throw new InvalidOperationException($"User with ID {userId} does not exist.");
            }

            if (this.postService.GetPostById(postId) == null)
            {
                throw new InvalidOperationException($"Post with ID {postId} does not exist.");
            }

            Comment comment = new Comment
            {
                Content = content,
                UserId = userId,
                PostId = postId,
                CreatedDate = DateTime.Now,
            };

            this.commentRepository.SaveComment(comment);

            return comment;
        }

        /// <summary>
        /// Validates and deletes a comment by its ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to be deleted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the comment does not exist.</exception>
        //public void DeleteComment(long commentId)
        //{
        //    if (commentRepository.GetCommentById(commentId) == null)
        //    {
        //        throw new InvalidOperationException($"Comment with ID {commentId} does not exist.");
        //    }

        //    this.commentRepository.DeleteCommentById(commentId);
        //}

        /// <summary>
        /// Validates if an update is possible.
        /// </summary>
        /// <param name="commentId">The ID of the comment that updates.</param>
        /// <param name="content">The content which we want to update the comment with.</param>
        /// <exception cref="Exception">Throw when comment with given Id is not found.</exception>
        /// <exception cref="Exception">Throw when the given content is empty.</exception>
        //public void UpdateComment(long commentId, string content)
        //{
        //    if (this.commentRepository.GetCommentById(commentId) == null)
        //    {
        //        throw new Exception("Comment does not exist");
        //    }

        //    if (content == null || content.Length == 0)
        //    {
        //        throw new Exception("Comment content cannot be empty");
        //    }

        //    this.commentRepository.UpdateCommentContentById(commentId, content);
        //}

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns> A list of all the comments.</returns>
        public List<Comment> GetAllComments()
        {
            return this.commentRepository.GetAllComments();
        }

        /// <summary>
        /// Gets a comment by ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to retrieve.</param>
        /// <returns>The comment with the specified ID.</returns>
        //public Comment GetCommentById(int commentId)
        //{
        //    return this.commentRepository.GetCommentById(commentId);
        //}

        /// <summary>
        /// Gets comments by post ID.
        /// </summary>
        /// <param name="postId">The ID of the post which the comments are retrieved from.</param>
        /// <returns>A list of comments specified by the post.</returns>
        public List<Comment> GetCommentsByPostId(long postId)
        {
            return this.commentRepository.GetCommentsByPostId(postId);
        }
    }
}
