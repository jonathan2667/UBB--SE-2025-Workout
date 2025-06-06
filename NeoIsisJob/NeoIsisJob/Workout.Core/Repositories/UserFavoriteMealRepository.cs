using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;

namespace Workout.Core.Repositories
{
    public class UserFavoriteMealRepository
    {
        private readonly WorkoutDbContext context;

        public UserFavoriteMealRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        // Get all favorite meals for a specific user
        public async Task<IEnumerable<UserFavoriteMealModel>> GetUserFavoritesAsync(int userId)
        {
            return await context.UserFavoriteMeals
                .Include(fm => fm.Meal)
                .Where(fm => fm.UserID == userId)
                .ToListAsync();
        }

        // Add a meal to user's favorites (if not already present)
        public async Task<UserFavoriteMealModel> AddToFavoritesAsync(int userId, int mealId)
        {
            var existing = await context.UserFavoriteMeals
                .FirstOrDefaultAsync(fm => fm.UserID == userId && fm.MealID == mealId);
            if (existing != null)
                return existing;
            var favorite = new UserFavoriteMealModel { UserID = userId, MealID = mealId };
            context.UserFavoriteMeals.Add(favorite);
            await context.SaveChangesAsync();
            return favorite;
        }

        // Remove a meal from user's favorites
        public async Task<bool> RemoveFromFavoritesAsync(int userId, int mealId)
        {
            var favorite = await context.UserFavoriteMeals
                .FirstOrDefaultAsync(fm => fm.UserID == userId && fm.MealID == mealId);
            if (favorite == null)
                return false;
            context.UserFavoriteMeals.Remove(favorite);
            await context.SaveChangesAsync();
            return true;
        }

        // Check if a meal is a favorite for a user
        public async Task<bool> IsMealFavoriteAsync(int userId, int mealId)
        {
            return await context.UserFavoriteMeals
                .AnyAsync(fm => fm.UserID == userId && fm.MealID == mealId);
        }
    }
} 