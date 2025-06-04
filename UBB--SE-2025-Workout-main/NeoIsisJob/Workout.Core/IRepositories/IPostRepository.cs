using ServerLibraryProject.Enums;
using ServerLibraryProject.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface IPostRepository
    {
        //bool DeletePostById(long postId);
        List<Post> GetAllPosts();
        List<Post> GetPostsByGroupId(long groupId);
        Post GetPostById(long postId);
        List<Post> GetPostsByUserId(int userId);
        List<Post> GetPostsGroupsFeed(int userId);
        List<Post> GetPostsHomeFeed(int userId);
        void SavePost(Post entity);
        //bool UpdatePostById(long postId, string title, string content, PostVisibility visibility, PostTag tag);
    }
}