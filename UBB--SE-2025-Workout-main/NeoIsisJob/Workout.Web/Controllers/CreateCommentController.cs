namespace ServerMVCProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Models;
    using ServerMVCProject.Models;
    using ServerLibraryProject.Interfaces;

    [Route("comments")]
    public class CreateCommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CreateCommentController> _logger;
        private readonly IPostService _postService; // Add this for validation

        public CreateCommentController(
            ICommentService commentService,
            ILogger<CreateCommentController> logger,
            IPostService postService) // Add post service
        {
            _commentService = commentService;
            _logger = logger;
            _postService = postService;
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create(long postId)
        {
            _logger.LogInformation($"Opening create comment form for post ID: {postId}");

            // Validate that the post exists
            if (postId <= 0 || _postService.GetPostById(postId) == null)
            {
                return BadRequest($"Invalid or non-existent post ID: {postId}");
            }

            var model = new CreateCommentViewModel
            {
                PostId = postId
            };
            return View(model);
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create(CreateCommentViewModel model)
        {
            try
            {
                _logger.LogInformation($"Attempting to create comment for post ID: {model.PostId}");

                // Ensure the PostId is valid
                if (model.PostId <= 0 || _postService.GetPostById(model.PostId) == null)
                {
                    ModelState.AddModelError(string.Empty, $"Invalid or non-existent post ID: {model.PostId}");
                    return View(model);
                }

                var userIdString = HttpContext.Session.GetString("UserId");
                long userId;

                if (string.IsNullOrEmpty(userIdString))
                {
                    _logger.LogWarning("User ID not found in session, using default value 1");
                    userId = 1; // Default user for testing
                }
                else
                {
                    userId = long.Parse(userIdString);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state");
                    return View(model);
                }

                // Use the service directly
                var comment = _commentService.AddComment(model.Content, userId, model.PostId);

                _logger.LogInformation($"Comment created successfully with ID: {comment.Id}");

                TempData["SuccessMessage"] = "Comment created successfully!";
                return RedirectToAction("Index", "ViewPosts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }
    }
}
