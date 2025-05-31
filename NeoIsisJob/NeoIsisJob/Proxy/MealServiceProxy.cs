// <copyright file="MealServiceProxy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Proxy for meal service with fallback data.
    /// </summary>
    public class MealServiceProxy : IService<MealModel>
    {
        /// <summary>
        /// Sample meals with proper nutritional data for testing.
        /// </summary>
        private static readonly List<MealModel> SampleMeals = new List<MealModel>
        {
            new MealModel
            {
                Id = 1,
                Name = "Chicken Salad",
                Type = "Salad",
                ImageUrl = "/images/chickensalad.jpg",
                CookingLevel = "Easy",
                CookingTimeMins = 15,
                Directions = "Mix grilled chicken with fresh lettuce, tomatoes, and croutons. Serve with dressing.",
                Calories = 450,
                Proteins = 32.0,
                Carbohydrates = 35.5,
                Fats = 18.2,
                Ingredients = new List<IngredientModel>
                {
                    new IngredientModel { Id = 1, Name = "Lettuce" },
                    new IngredientModel { Id = 2, Name = "Tomato" },
                    new IngredientModel { Id = 3, Name = "Chicken" },
                    new IngredientModel { Id = 5, Name = "Croutons" }
                }
            },
            new MealModel
            {
                Id = 2,
                Name = "Veggie Cheese Wrap",
                Type = "Vegetarian",
                ImageUrl = "/images/veggiedelight.jpg",
                CookingLevel = "Easy",
                CookingTimeMins = 10,
                Directions = "Wrap fresh vegetables and cheese in a tortilla. Cut in half and serve.",
                Calories = 320,
                Proteins = 15.3,
                Carbohydrates = 42.0,
                Fats = 12.7,
                Ingredients = new List<IngredientModel>
                {
                    new IngredientModel { Id = 1, Name = "Lettuce" },
                    new IngredientModel { Id = 2, Name = "Tomato" },
                    new IngredientModel { Id = 4, Name = "Cheese" }
                }
            },
            new MealModel
            {
                Id = 3,
                Name = "Protein Oatmeal",
                Type = "Breakfast",
                ImageUrl = "/images/proteinoatmeal.jpg",
                CookingLevel = "Easy",
                CookingTimeMins = 5,
                Directions = "Cook oats with protein powder and top with berries.",
                Calories = 280,
                Proteins = 25.0,
                Carbohydrates = 35.0,
                Fats = 6.5,
                Ingredients = new List<IngredientModel>()
            },
            new MealModel
            {
                Id = 4,
                Name = "Grilled Chicken & Rice",
                Type = "Lunch",
                ImageUrl = "/images/chickenrice.jpg",
                CookingLevel = "Medium",
                CookingTimeMins = 30,
                Directions = "Grill seasoned chicken breast and serve with steamed rice and vegetables.",
                Calories = 520,
                Proteins = 45.0,
                Carbohydrates = 55.0,
                Fats = 8.0,
                Ingredients = new List<IngredientModel>
                {
                    new IngredientModel { Id = 3, Name = "Chicken" }
                }
            },
            new MealModel
            {
                Id = 5,
                Name = "Tuna Sandwich",
                Type = "Lunch",
                ImageUrl = "/images/tunasandwich.jpg",
                CookingLevel = "Easy",
                CookingTimeMins = 8,
                Directions = "Mix tuna with mayo, add lettuce and tomato between bread slices.",
                Calories = 380,
                Proteins = 28.0,
                Carbohydrates = 32.0,
                Fats = 15.0,
                Ingredients = new List<IngredientModel>
                {
                    new IngredientModel { Id = 1, Name = "Lettuce" },
                    new IngredientModel { Id = 2, Name = "Tomato" }
                }
            },
            new MealModel
            {
                Id = 6,
                Name = "Greek Yogurt Bowl",
                Type = "Snack",
                ImageUrl = "/images/yogurtbowl.jpg",
                CookingLevel = "Easy",
                CookingTimeMins = 3,
                Directions = "Top Greek yogurt with berries, nuts, and honey.",
                Calories = 180,
                Proteins = 20.0,
                Carbohydrates = 18.0,
                Fats = 4.5,
                Ingredients = new List<IngredientModel>()
            },
            new MealModel
            {
                Id = 7,
                Name = "Salmon & Vegetables",
                Type = "Dinner",
                ImageUrl = "/images/salmon.jpg",
                CookingLevel = "Medium",
                CookingTimeMins = 25,
                Directions = "Bake salmon with seasonal vegetables and herbs.",
                Calories = 480,
                Proteins = 40.0,
                Carbohydrates = 12.0,
                Fats = 28.0,
                Ingredients = new List<IngredientModel>()
            },
            new MealModel
            {
                Id = 8,
                Name = "Protein Smoothie",
                Type = "Snack",
                ImageUrl = "/images/smoothie.jpg",
                CookingLevel = "Easy",
                CookingTimeMins = 5,
                Directions = "Blend protein powder with banana, berries, and almond milk.",
                Calories = 220,
                Proteins = 25.0,
                Carbohydrates = 22.0,
                Fats = 3.0,
                Ingredients = new List<IngredientModel>()
            }
        };

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetAllAsync()
        {
            await Task.Delay(100); // Simulate async operation
            return SampleMeals;
        }

        /// <inheritdoc/>
        public async Task<MealModel> GetByIdAsync(int id)
        {
            await Task.Delay(50); // Simulate async operation
            return SampleMeals.FirstOrDefault(m => m.Id == id);
        }

        /// <inheritdoc/>
        public async Task<MealModel> CreateAsync(MealModel meal)
        {
            await Task.Delay(100); // Simulate async operation
            meal.Id = SampleMeals.Count > 0 ? SampleMeals.Max(m => m.Id) + 1 : 1;
            SampleMeals.Add(meal);
            return meal;
        }

        /// <inheritdoc/>
        public async Task<MealModel> UpdateAsync(MealModel meal)
        {
            await Task.Delay(100); // Simulate async operation
            var existingMeal = SampleMeals.FirstOrDefault(m => m.Id == meal.Id);
            if (existingMeal != null)
            {
                var index = SampleMeals.IndexOf(existingMeal);
                SampleMeals[index] = meal;
                return meal;
            }
            throw new ArgumentException($"Meal with ID {meal.Id} not found.");
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            await Task.Delay(100); // Simulate async operation
            var meal = SampleMeals.FirstOrDefault(m => m.Id == id);
            if (meal != null)
            {
                SampleMeals.Remove(meal);
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetFilteredAsync(IFilter filter)
        {
            await Task.Delay(50); // Simulate async operation
            
            if (filter is not MealFilter mealFilter)
            {
                return SampleMeals;
            }

            var filteredMeals = SampleMeals.AsQueryable();

            if (!string.IsNullOrEmpty(mealFilter.Type))
            {
                filteredMeals = filteredMeals.Where(m => m.Type.Equals(mealFilter.Type, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(mealFilter.CookingLevel))
            {
                filteredMeals = filteredMeals.Where(m => m.CookingLevel.Equals(mealFilter.CookingLevel, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(mealFilter.CookingTimeRange))
            {
                filteredMeals = mealFilter.CookingTimeRange.ToLower() switch
                {
                    "quick" => filteredMeals.Where(m => m.CookingTimeMins <= 15),
                    "medium" => filteredMeals.Where(m => m.CookingTimeMins > 15 && m.CookingTimeMins <= 45),
                    "long" => filteredMeals.Where(m => m.CookingTimeMins > 45),
                    _ => filteredMeals
                };
            }

            if (!string.IsNullOrEmpty(mealFilter.CalorieRange))
            {
                filteredMeals = mealFilter.CalorieRange.ToLower() switch
                {
                    "low" => filteredMeals.Where(m => m.Calories <= 300),
                    "medium" => filteredMeals.Where(m => m.Calories > 300 && m.Calories <= 500),
                    "high" => filteredMeals.Where(m => m.Calories > 500),
                    _ => filteredMeals
                };
            }

            return filteredMeals.ToList();
        }

        /// <summary>
        /// Gets meals by type.
        /// </summary>
        /// <param name="type">The meal type.</param>
        /// <returns>A collection of meals of the specified type.</returns>
        public async Task<IEnumerable<MealModel>> GetByTypeAsync(string type)
        {
            await Task.Delay(50); // Simulate async operation
            return SampleMeals.Where(m => m.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }
}
