namespace Workout.Core.IServices
{
    using Workout.Core.Enums;
    using Workout.Core.Models;

    public interface IPostService
    {
        void AddPost(string title, string content, int userId, long? groupId, PostVisibility postVisibility, PostTag postTag);

        //void DeletePost(long id);

        List<Post> GetPostsByUserId(int userId);

        List<Post> GetAllPosts();

        List<Post> GetPostsByGroupId(long groupId);

        Post GetPostById(long id);

        //List<Post> GetPostsGroupsFeed(long userId);

        List<Post> GetPostsHomeFeed(int userId);

        //void UpdatePost(long id, string title, string description, PostVisibility visibility, PostTag tag);

        //void SavePost(Post entity);
    }
}