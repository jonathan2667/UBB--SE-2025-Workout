using ServerLibraryProject.Interfaces;
using Workout.Core.Data;
using Workout.Core.Enums;
using Workout.Core.Models;

namespace ServerLibraryProject.Repositories
{
    /// <summary>
    /// Repository for managing posts in the database.
    /// </summary>
    public class PostRepository : IPostRepository
    {
        private readonly WorkoutDbContext dbContext;

        public PostRepository(WorkoutDbContext context)
        {
            this.dbContext = context;
        }

        public Post GetPostById(long postId)
        {
            try
            {
                return dbContext.Posts.FirstOrDefault(p => p.Id == postId);
            }
            catch
            {
                throw new Exception("Error retrieving the post by ID");
            }
        }

        public List<Post> GetAllPosts()
        {
            return dbContext.Posts.ToList();
        }

        public void SavePost(Post entity)
        {
            try
            {
                this.dbContext.Posts.Add(entity);
                this.dbContext.SaveChanges();
            }
            catch
            {
                throw new Exception("Error saving the post");
            }
        }

        //public bool UpdatePostById(long postId, string title, string content, PostVisibility visibility, PostTag tag)
        //{
        //    var post = this.dbContext.Posts.Find(postId);
        //    if (post != null)
        //    {
        //        post.Title = title;
        //        post.Content = content;
        //        post.Visibility = visibility;
        //        post.Tag = tag;
        //        this.dbContext.SaveChanges();
        //        return true;
        //    }

        //    return false;
        //}

        //public bool DeletePostById(long postId)
        //{
        //    var post = this.dbContext.Posts.Find(postId);
        //    if (post != null)
        //    {
        //        this.dbContext.Posts.Remove(post);
        //        this.dbContext.SaveChanges();
        //        return true;
        //    }

        //    return false;
        //}

        public List<Post> GetPostsHomeFeed(int userId)
        {
            var postsQuery = from post in dbContext.Posts
                             where post.UserId == userId
                                || post.Visibility == PostVisibility.Public
                                || dbContext.UserFollowers.Any(userFollower =>
                                       (userFollower.FollowerId == userId
                                    || userFollower.UserId == post.UserId)
                                    && (post.Visibility == PostVisibility.Friends))
                             orderby post.CreatedDate descending
                             select post;


            return postsQuery.ToList();
        }

        public List<Post> GetPostsGroupsFeed(int userId)
        {
            var postsQuery = from post in dbContext.Posts
                             where dbContext.GroupUsers.Any(groupUser =>
                                   groupUser.UserId == userId && groupUser.GroupId == post.GroupId)
                             orderby post.CreatedDate descending
                             select post;

            return postsQuery.ToList();
        }

        public List<Post> GetPostsByUserId(int userId)
        {
            return dbContext.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
        }

        public List<Post> GetPostsByGroupId(long groupId)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
        }

    }
}
