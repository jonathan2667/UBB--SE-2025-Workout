using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Repositories;

namespace Workout.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService _classService;
        private readonly IUserClassService _userClassService;

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

        public async Task<IActionResult> Enroll(int id)
        {
            // TODO: Implement enrollment logic
            return RedirectToAction(nameof(Details), new { id });
        }
    }
} 