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
        List<Post> GetPostsByUserId(long userId);
        List<Post> GetPostsGroupsFeed(long userId);
        List<Post> GetPostsHomeFeed(long userId);
        void SavePost(Post entity);
        //bool UpdatePostById(long postId, string title, string content, PostVisibility visibility, PostTag tag);
    }
}