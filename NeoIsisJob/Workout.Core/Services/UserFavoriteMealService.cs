using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.Models;
using Workout.Core.Repositories;

namespace Workout.Core.Services
{
    public class UserFavoriteMealService
    {
        private readonly UserFavoriteMealRepository repository;

        public UserFavoriteMealService(UserFavoriteMealRepository repository)
        {
            this.repository = repository;
        }

        // Get all favorite meals for a user
        public async Task<IEnumerable<UserFavoriteMealModel>> GetUserFavoritesAsync(int userId)
        {
            return await repository.GetUserFavoritesAsync(userId);
        }

        // Add a meal to favorites with validation
        public async Task<UserFavoriteMealModel> AddToFavoritesAsync(int userId, int mealId)
        {
            // Prevent duplicates (repository already checks, but you can add extra logic here if needed)
            var isFavorite = await repository.IsMealFavoriteAsync(userId, mealId);
            if (isFavorite)
                throw new System.InvalidOperationException("Meal is already in favorites.");
            return await repository.AddToFavoritesAsync(userId, mealId);
        }

        // Remove a meal from favorites
        public async Task<bool> RemoveFromFavoritesAsync(int userId, int mealId)
        {
            return await repository.RemoveFromFavoritesAsync(userId, mealId);
        }

        // Check if a meal is a favorite
        public async Task<bool> IsMealFavoriteAsync(int userId, int mealId)
        {
            return await repository.IsMealFavoriteAsync(userId, mealId);
        }
    }
} 