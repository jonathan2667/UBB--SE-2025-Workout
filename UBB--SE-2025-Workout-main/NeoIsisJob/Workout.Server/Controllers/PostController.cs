namespace ServerAPIProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;

    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IReactionService reactionService;
        private readonly ICommentService commentService;

        public PostController(IPostService postService, IReactionService reactionService, ICommentService commentService)
        {
            this.postService = postService;
            this.reactionService = reactionService;
            this.commentService = commentService;
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAllPosts()
        {
            try
            {
                return this.Ok(this.postService.GetAllPosts());
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving posts: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Post> GetPostById(long id)
        {
            try
            {
                var post = this.postService.GetPostById(id);
                if (post == null)
                    return this.NotFound($"Post with ID {id} not found.");

                return this.Ok(post);
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving post: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<Post>> GetPostsByUserId(int userId)
        {
            try
            {
                return this.Ok(this.postService.GetPostsByUserId(userId));
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving user's posts: {ex.Message}");
            }
        }

        [HttpGet("group/{groupId}")]
        public ActionResult<List<Post>> GetPostsByGroupId(long groupId)
        {
            try
            {
                return this.Ok(postService.GetPostsByGroupId(groupId));
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving group's posts: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}/homefeed")]
        public ActionResult<List<Post>> GetHomeFeed(int userId)
        {
            try
            {
                return this.Ok(this.postService.GetPostsHomeFeed(userId));
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving home feed: {ex.Message}");
            }
        }

        //[HttpGet("user/{userId}/groupfeed")]
        //public ActionResult<List<Post>> GetGroupFeed(long userId)
        //{
        //    try
        //    {
        //        return this.Ok(this.postService.GetPostsGroupsFeed(userId));
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.StatusCode(404, $"Error retrieving group feed: {ex.Message}");
        //    }
        //}

        [HttpPost]
        public IActionResult SavePost(Post post)
        {
            try
            {
                if (post == null)
                    return this.BadRequest("Post data cannot be null.");

                this.postService.AddPost(post.Title, post.Content, post.UserId, post.GroupId, post.Visibility, post.Tag);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error saving post: {ex.Message}");
            }
        }

        [HttpGet("{postId}/reactions")]
        public ActionResult<List<Reaction>> GetReactionsByPost(long postId)
        {
            return this.reactionService.GetReactionsByPostId(postId);
        }

        [HttpGet("{postId}/user/{userId}/reaction")]
        public ActionResult<Reaction> GetUserPostReaction(int userId, long postId)
        {
            try
            {
                return this.reactionService.GetReaction(userId, postId);
            }
            catch (Exception ex)
            {
                return this.NotFound($"Reaction not found for user {userId} on post {postId}. Error: {ex.Message}");
            }
        }

        //[HttpDelete("{postId}/user/{userId}/reaction")]
        //public IActionResult DeleteReaction(long userId, long postId)
        //{
        //    try
        //    {
        //        this.reactionService.DeleteReaction(userId, postId);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.NotFound($"Reaction not found for user {userId} on post {postId}. Error: {ex.Message}");
        //    }
        //}

        [HttpGet("{postId}/comments")]
        public ActionResult<List<Comment>> GetCommentsByPostId(long postId)
        {
            var comments = this.commentService.GetCommentsByPostId(postId);
            return this.Ok(comments);
        }
    }
}
