using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace NeoIsisJob.Proxy
{
    public class FavoriteMealServiceProxy
    {
        private readonly UserFavoriteMealService _favoriteMealService;
        private readonly int userId = 1; // Replace with actual user ID from session/auth

        public FavoriteMealServiceProxy()
        {
            _favoriteMealService = App.Services.GetRequiredService<UserFavoriteMealService>();
        }

        public async Task<IEnumerable<UserFavoriteMealModel>> GetAllAsync()
        {
            return await _favoriteMealService.GetUserFavoritesAsync(userId);
        }

        public async Task<UserFavoriteMealModel> CreateAsync(UserFavoriteMealModel favoriteMeal)
        {
            return await _favoriteMealService.AddToFavoritesAsync(favoriteMeal.UserID, favoriteMeal.MealID);
        }

        public async Task<bool> DeleteAsync(int mealId)
        {
            return await _favoriteMealService.RemoveFromFavoritesAsync(userId, mealId);
        }
    }
} 