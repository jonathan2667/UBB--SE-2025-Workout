using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace NeoIsisJob.ViewModels.Shop
{
    public class FavouriteMealsViewModel
    {
        private readonly UserFavoriteMealService _favoriteMealService;
        private readonly int userId = 1; // Replace with actual user ID from session/auth

        public FavouriteMealsViewModel()
        {
            _favoriteMealService = App.Services.GetRequiredService<UserFavoriteMealService>();
        }

        public async Task<IEnumerable<UserFavoriteMealModel>> GetAllFavouriteMealsAsync()
        {
            return await _favoriteMealService.GetUserFavoritesAsync(userId);
        }

        public async Task<UserFavoriteMealModel> AddMealToFavourites(MealModel meal)
        {
            return await _favoriteMealService.AddToFavoritesAsync(userId, meal.Id);
        }

        public async Task<bool> RemoveMealFromFavourites(int mealId)
        {
            return await _favoriteMealService.RemoveFromFavoritesAsync(userId, mealId);
        }

        public async Task<bool> IsMealFavorite(int mealId)
        {
            return await _favoriteMealService.IsMealFavoriteAsync(userId, mealId);
        }
    }
} 