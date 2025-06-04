// localhost/posts/create
namespace ServerMVCProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;
    using ServerMVCProject.Models;
    using Workout.Core.IServices;

    [Route("posts")]
    public class CreatePostController : Controller
    {
        private readonly IPostService postService;
        private readonly IGroupService groupService;

        public CreatePostController(IPostService postService, IGroupService groupService)
        {
            this.postService = postService;
            this.groupService = groupService;
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            string userIdStr = HttpContext.Session.GetString("UserId");

            int userId = int.Parse(userIdStr);

            var userGroups = this.groupService.GetUserGroups(userId);

            ViewBag.UserGroups = userGroups.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).ToList();

            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string userIdStr = this.HttpContext.Session.GetString("UserId");
          

            int userId = int.Parse(userIdStr);

            var userGroups = this.groupService.GetUserGroups(userId);
            ViewBag.UserGroups = userGroups.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).ToList();

            Post newPost = new Post
            {
                Title = model.Title,
                Content = model.Content,
                Visibility = model.Visibility,
                Tag = model.Tag,
                UserId = userId,
                GroupId = model.GroupId != 0 ? model.GroupId:null,

                CreatedDate = DateTime.UtcNow
            };
            try
            {
                postService.AddPost(newPost.Title, newPost.Content, newPost.UserId, newPost.GroupId, newPost.Visibility, newPost.Tag);
                ViewBag.Message = "Post created successfully!";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error creating post: {ex.Message}";
                return View(model);
            }

        }

        public IActionResult Success()
        {
            return View();
        }
    }
}