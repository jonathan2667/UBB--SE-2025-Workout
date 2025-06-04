//localhost/posts
namespace ServerMVCProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;
    using ServerLibraryProject.Repositories;

    [Route("posts")]
    public class ViewPostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IReactionRepository reactionRepository;

        public ViewPostsController(IPostService postService, IReactionRepository reactionRepository)
        {
            this.postService = postService;
            this.reactionRepository = reactionRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            string userIdStr = this.HttpContext.Session.GetString("UserId");

            int userId = int.Parse(userIdStr);

            List<Post> posts = this.postService.GetPostsHomeFeed(userId);
            return this.View(posts);
        }


        [HttpPost("react")]
        public JsonResult ReactAjax(long postId, string type)
        {
            try
            {
                int userId = 1; // Hardcoded user ID for testing
                if (!Enum.TryParse<ReactionType>(type, out var reactionType))
                    return Json(new { success = false, error = "Invalid reaction type" });

                var existing = reactionRepository.GetReaction(userId, postId);
                if (existing == null)
                {
                    reactionRepository.Add(new Reaction { UserId = userId, PostId = postId, Type = reactionType });
                }
                else if (existing.Type == reactionType)
                {
                    reactionRepository.Delete(userId, postId);
                }
                else
                {
                    reactionRepository.Update(userId, postId, reactionType);
                }

                var reactions = reactionRepository.GetReactionsByPostId(postId);
                return Json(new
                {
                    success = true,
                    like = reactions.Count(r => r.Type == ReactionType.Like),
                    love = reactions.Count(r => r.Type == ReactionType.Love),
                    laugh = reactions.Count(r => r.Type == ReactionType.Laugh),
                    anger = reactions.Count(r => r.Type == ReactionType.Anger)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}