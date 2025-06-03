using ServerLibraryProject.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface ICommentRepository
    {
        //void DeleteCommentById(long id);

        List<Comment> GetAllComments();

        //Comment GetCommentById(long id);

        List<Comment> GetCommentsByPostId(long postId);

        void SaveComment(Comment entity);

        //void UpdateCommentContentById(long id, string content);
    }
}