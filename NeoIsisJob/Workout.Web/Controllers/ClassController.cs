using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.Services;

namespace Workout.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService _classService;
        private readonly IUserClassService _userClassService;

		private int GetCurrentUserId()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			//return int.Parse(userId);
			return 1; // hardcodat pana se repara UserId
		}

		private string GetCurrentUserName() => User.Identity?.Name;

		public ClassController(
            IClassService classService, IUserClassService userClassService)
        {
            _classService = classService;
            _userClassService = userClassService;
        }


        public async Task<IActionResult> Index()
        {
            var classes = await _classService.GetAllClassesAsync();
            return View(classes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var classModel = await _classService.GetClassByIdAsync(id);
            if (classModel == null)
            {
                return NotFound();
            }
            return View(classModel);
        }

		public async Task<IActionResult> Enroll(int cid, DateTime selectedDate)
		{
			if (selectedDate.Date < DateTime.Today)
			{
				TempData["Error"] = "Please choose a valid date.";
				return RedirectToAction("Details", new { id = cid });
			}

			var classModel = await _classService.GetClassByIdAsync(cid);
			if (classModel == null)
			{
				return NotFound();
			}

			var userClass = new UserClassModel
			{
				UID = GetCurrentUserId(), 
				CID = cid,
				Date = selectedDate
			};

			await _userClassService.AddUserClassAsync(userClass);
			TempData["Success"] = "true";

			return RedirectToAction("Details", new { id = cid });
		}
	}
} 