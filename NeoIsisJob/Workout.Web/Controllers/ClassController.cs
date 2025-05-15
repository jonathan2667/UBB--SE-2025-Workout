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
				UID = GetCurrentUserId(), // Replace with your logic to get current user ID
				CID = cid,
				Date = selectedDate
			};

			await _userClassService.AddUserClassAsync(userClass);
			TempData["Success"] = $"Successfully registered for {classModel.Name}.";

			return RedirectToAction("Details", new { id = cid });
		}



		//private async void ConfirmRegistration()
		//{
		//    if (SelectedClass == null)
		//    {
		//        DateError = "No class selected.";
		//        return;
		//    }

		//    // Validate date is not in the past
		//    if (SelectedDate.Date < DateTime.Today)
		//    {
		//        DateError = "Please choose a valid date (today or future)";
		//        return;
		//    }

		//    try
		//    {
		//        int currentUserId = GetCurrentUserId();
		//        Debug.WriteLine($"[ClassesViewModel] Registering user {currentUserId} for class {SelectedClass.CID} on {SelectedDate.Date}");

		//        var userClass = new UserClassModel
		//        {
		//            UID = currentUserId,
		//            CID = SelectedClass.CID,
		//            Date = SelectedDate.Date
		//        };

		//        await userClassService.AddUserClassAsync(userClass);
		//        DateError = string.Empty; // Clear error if successful
		//        Debug.WriteLine($"[ClassesViewModel] Successfully registered for class {SelectedClass.Name}");
		//        IsRegisterPopupOpen = false;
		//    }
		//    catch (Exception ex)
		//    {
		//        DateError = $"Registration failed: {ex.Message}";
		//        Debug.WriteLine($"[ClassesViewModel] Registration failed: {ex.Message}");
		//    }
		//}
	}
} 