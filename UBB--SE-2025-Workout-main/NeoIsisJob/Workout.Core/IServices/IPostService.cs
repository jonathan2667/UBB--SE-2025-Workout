namespace ServerLibraryProject.Interfaces
{
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Models;

    public interface IPostService
    {
        void AddPost(string title, string content, long userId, long? groupId, PostVisibility postVisibility, PostTag postTag);

        //void DeletePost(long id);

        List<Post> GetPostsByUserId(long userId);

        List<Post> GetAllPosts();

        List<Post> GetPostsByGroupId(long groupId);

        Post GetPostById(long id);

        //List<Post> GetPostsGroupsFeed(long userId);

        List<Post> GetPostsHomeFeed(long userId);

        //void UpdatePost(long id, string title, string description, PostVisibility visibility, PostTag tag);

        //void SavePost(Post entity);
    }
}