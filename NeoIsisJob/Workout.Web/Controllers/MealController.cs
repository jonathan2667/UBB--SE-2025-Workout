using Microsoft.AspNetCore.Mvc;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Web.ViewModels.Meal;
using Workout.Core.IServices;

namespace Workout.Web.Controllers
{
    public class MealController : Controller
    {
        private readonly IService<MealModel> _mealService;

        public MealController(IService<MealModel> mealService)
        {
            _mealService = mealService;
        }

        public async Task<IActionResult> Index()
        {
            var meals = await _mealService.GetAllAsync();
            return View(meals);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMealViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var meal = new MealModel
                    {
                        Name = model.Name,
                        Type = model.Type,
                        ImageUrl = model.ImageUrl,
                        CookingLevel = model.CookingLevel,
                        CookingTimeMins = model.CookingTimeMins,
                        Directions = model.Directions,
                        Calories = model.Calories,
                        Proteins = model.Proteins,
                        Carbohydrates = model.Carbohydrates,
                        Fats = model.Fats,
                        Ingredients = new List<IngredientModel>()
                    };

                    await _mealService.CreateAsync(meal);
                    TempData["SuccessMessage"] = "Meal created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Failed to create meal: " + ex.Message;
                }
            }
            return View(model);
        }
    }
} 