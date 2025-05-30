using Microsoft.AspNetCore.Mvc;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Web.ViewModels.Meal;
using Workout.Core.IServices;
using Workout.Core.Utils.Filters;
using Workout.Web.Filters;

namespace Workout.Web.Controllers
{
    [AuthorizeUser]
    public class MealController : Controller
    {
        private readonly IService<MealModel> _mealService;
        private readonly UserFavoriteMealService _favoriteMealService;

        public MealController(IService<MealModel> mealService, UserFavoriteMealService favoriteMealService)
        {
            _mealService = mealService;
            _favoriteMealService = favoriteMealService;
        }

        public async Task<IActionResult> Index(MealFilter filter)
        {
            var viewModel = new MealIndexViewModel
            {
                Filter = filter ?? new MealFilter()
            };

            try
            {
                // If no filters are applied, get all meals
                if (IsFilterEmpty(filter))
                {
                    viewModel.Meals = await _mealService.GetAllAsync();
                }
                else
                {
                    viewModel.Meals = await _mealService.GetFilteredAsync(filter);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load meals: " + ex.Message;
                viewModel.Meals = new List<MealModel>();
            }

            return View(viewModel);
        }

        private bool IsFilterEmpty(MealFilter filter)
        {
            return filter == null ||
                    (string.IsNullOrEmpty(filter.Type) &&
                     string.IsNullOrEmpty(filter.CookingLevel) &&
                     string.IsNullOrEmpty(filter.CookingTimeRange) &&
                     string.IsNullOrEmpty(filter.CalorieRange));
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new MealIndexViewModel
            {
                Filter = filter ?? new MealFilter()
            };

            try
            {
                // If no filters are applied, get all meals
                if (IsFilterEmpty(filter))
                {
                    viewModel.Meals = await _mealService.GetAllAsync();
                }
                else
                {
                    viewModel.Meals = await _mealService.GetFilteredAsync(filter);
                }

                // Get favorite meal IDs for the current user
                int userId = 0;
                if (HttpContext.Session.GetString("UserId") != null)
                    userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var favoriteIds = userId > 0 ? (await _favoriteMealService.GetUserFavoritesAsync(userId)).Select(f => f.MealID).ToList() : new List<int>();
                ViewBag.FavoriteMealIds = favoriteIds;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load meals: " + ex.Message;
                viewModel.Meals = new List<MealModel>();
            }

            return View(viewModel);
        }

        private bool IsFilterEmpty(MealFilter filter)
        {
            return filter == null ||
                   (string.IsNullOrEmpty(filter.Type) &&
                    string.IsNullOrEmpty(filter.CookingLevel) &&
                    string.IsNullOrEmpty(filter.CookingTimeRange) &&
                    string.IsNullOrEmpty(filter.CalorieRange));
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

        [AuthorizeUser]
        public async Task<IActionResult> Favorites()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var favorites = await _favoriteMealService.GetUserFavoritesAsync(userId);
            return View(favorites);
        }

        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int mealId)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            try
            {
                await _favoriteMealService.AddToFavoritesAsync(userId, mealId);
                TempData["SuccessMessage"] = "Meal added to favorites!";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int mealId)
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            await _favoriteMealService.RemoveFromFavoritesAsync(userId, mealId);
            TempData["SuccessMessage"] = "Meal removed from favorites!";
            return RedirectToAction("Favorites");
        }
    }
} 