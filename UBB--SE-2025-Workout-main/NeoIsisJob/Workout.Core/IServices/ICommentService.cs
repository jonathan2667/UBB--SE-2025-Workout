namespace Workout.Core.IServices
{
    using ServerLibraryProject.Models;

    public interface ICommentService
    {
        List<Comment> GetAllComments();

        //Comment GetCommentById(int commentId);

        List<Comment> GetCommentsByPostId(long postId);

        Comment AddComment(string content, int userId, long postId);

        //void DeleteComment(long commentId);

        //void UpdateComment(long commentId, string content);
    }
}