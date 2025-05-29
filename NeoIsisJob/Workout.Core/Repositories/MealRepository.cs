// <copyright file="MealRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Represents a repository for managing meals in the database.
    /// </summary>
    public class MealRepository : IRepository<MealModel>
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public MealRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<MealModel> CreateAsync(MealModel entity)
        {
            if (entity.Ingredients == null || !entity.Ingredients.Any())
            {
                entity.Ingredients = new List<IngredientModel>();
            }
            else
            {
                var ingredientIds = entity.Ingredients.Select(i => i.Id).ToList();
                var ingredients = await context.Ingredients
                    .Where(i => ingredientIds.Contains(i.Id))
                    .ToListAsync();

                // Optional: check if all requested IDs were found
                if (ingredients.Count != ingredientIds.Count)
                {
                    throw new ArgumentException("One or more ingredients not found");
                }

                entity.Ingredients = ingredients;
            }

            await this.context.Meals.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            MealModel? meal = await this.context.Meals.FindAsync(id);
            if (meal == null)
            {
                return false;
            }

            this.context.Meals.Remove(meal);
            int affectedRows = await this.context.SaveChangesAsync();
            return affectedRows > 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetAllAsync()
        {
            return await this.context.Meals
                .Include(m => m.Ingredients)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MealModel?> GetByIdAsync(int id)
        {
            return await this.context.Meals
                .Include(m => m.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc/>
        public async Task<MealModel> UpdateAsync(MealModel entity)
        {
            var existingMeal = await this.context.Meals
                .Include(m => m.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == entity.Id);

            if (existingMeal == null)
            {
                throw new ArgumentException("Meal not found", nameof(entity));
            }

            this.context.Entry(existingMeal).CurrentValues.SetValues(entity);

            existingMeal.Ingredients.Clear();

            if (entity.Ingredients != null && entity.Ingredients.Any())
            {
                var ingredientIds = entity.Ingredients.Select(i => i.Id).ToList();

                var ingredients = await context.Ingredients
                    .Where(i => ingredientIds.Contains(i.Id))
                    .ToListAsync();

                if (ingredients.Count != ingredientIds.Count)
                {
                    throw new ArgumentException("One or more ingredients not found");
                }

                foreach (var ingredient in ingredients)
                {
                    existingMeal.Ingredients.Add(ingredient);
                }
            }

            await this.context.SaveChangesAsync();

            await this.context.Entry(existingMeal).ReloadAsync();

            return existingMeal;
        }


        public async Task<IEnumerable<MealModel>> GetAllFilteredAsync(IFilter filter)
        {
            if (filter is not MealFilter mealFilter)
            {
                throw new ArgumentException("Invalid filter type", nameof(filter));
            }

            IQueryable<MealModel> query = this.context.Meals
                .Include(m => m.Ingredients)
                .Where(m =>
                    (string.IsNullOrEmpty(mealFilter.Type) || m.Type == mealFilter.Type) &&
                    (string.IsNullOrEmpty(mealFilter.CookingLevel) || m.CookingLevel == mealFilter.CookingLevel))
                .OrderBy(m => m.Id);

            // Apply cooking time range filter
            if (!string.IsNullOrEmpty(mealFilter.CookingTimeRange))
            {
                query = mealFilter.CookingTimeRange.ToLower() switch
                {
                    "quick" => query.Where(m => m.CookingTimeMins <= 15),
                    "medium" => query.Where(m => m.CookingTimeMins > 15 && m.CookingTimeMins <= 45),
                    "long" => query.Where(m => m.CookingTimeMins > 45),
                    _ => query
                };
            }

            // Apply calorie range filter
            if (!string.IsNullOrEmpty(mealFilter.CalorieRange))
            {
                query = mealFilter.CalorieRange.ToLower() switch
                {
                    "low" => query.Where(m => m.Calories <= 300),
                    "medium" => query.Where(m => m.Calories > 300 && m.Calories <= 600),
                    "high" => query.Where(m => m.Calories > 600),
                    _ => query
                };
            }

            return await query.ToListAsync();
        }

    }
}
