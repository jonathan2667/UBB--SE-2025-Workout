using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using NeoIsisJob.Proxy;

namespace NeoIsisJob.ViewModels.Shop
{
    public class FavouriteMealsViewModel
    {
        private readonly UserFavoriteMealServiceProxy _favoriteMealServiceProxy;
        private readonly int userId = 1; // Replace with actual user ID from session/auth

        public FavouriteMealsViewModel()
        {
            _favoriteMealServiceProxy = new UserFavoriteMealServiceProxy();
        }

        public async Task<IEnumerable<UserFavoriteMealModel>> GetAllFavouriteMealsAsync()
        {
            return await _favoriteMealServiceProxy.GetUserFavoritesAsync(userId);
        }

        public async Task<UserFavoriteMealModel> AddMealToFavourites(MealModel meal)
        {
            return await _favoriteMealServiceProxy.AddToFavoritesAsync(userId, meal.Id);
        }

        public async Task<bool> RemoveMealFromFavourites(int mealId)
        {
            return await _favoriteMealServiceProxy.RemoveFromFavoritesAsync(userId, mealId);
        }

        public async Task<bool> IsMealFavorite(int mealId)
        {
            return await _favoriteMealServiceProxy.IsMealFavoriteAsync(userId, mealId);
        }
    }
} 