using Workout.Core.Models;

namespace ServerMVCProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Workout.Core.IServices;
    using CommonGroup = Group;

    [Route("groups")]
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IUserService userService;
        public GroupsController(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            this.userService = userService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var groups = _groupService.GetAllGroups();
            return View(groups);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommonGroup group)
        {
            if (ModelState.IsValid)
            {
                _groupService.AddGroup(group.Name, group.Description);
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }
        [HttpPost("join/{id}")]
        public IActionResult Join(int id)
        {
            string userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            int userId = int.Parse(userIdStr);
            try
            {
                this.userService.JoinGroup(userId, id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", _groupService.GetAllGroups());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("exit/{id}")]
        public IActionResult Exit(int id)
        {
            string userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            int userId = int.Parse(userIdStr);
            try
            {
                this.userService.ExitGroup(userId, id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", _groupService.GetAllGroups());
            }
            return RedirectToAction(nameof(Index));
        }

        //[HttpGet("edit/{id}")]
        //public IActionResult Edit(int id)
        //{
        //    var group = _groupService.GetGroupById(id);
        //    if (group == null) return NotFound();
        //    return View(group);
        //}

        //[HttpPost("edit/{id}")]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(CommonGroup group)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _groupService.UpdateGroup(group.Id, group.Name, group.Description, group.Image, group.AdminId);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(group);
        //}

        //[HttpGet("delete/{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var group = _groupService.GetGroupById(id);
        //    if (group == null) return NotFound();
        //    return View(group);
        //}

        //[HttpPost("delete/{id}")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    _groupService.DeleteGroup(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}