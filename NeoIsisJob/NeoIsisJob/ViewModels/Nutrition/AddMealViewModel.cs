// <copyright file="AddMealViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.ViewModels.Nutrition
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using global::Workout.Core.Models;
    using NeoIsisJob.Proxy;

    /// <summary>
    /// ViewModel responsible for managing the creation of a meal.
    /// </summary>
    public class AddMealViewModel
    {
        private readonly MealServiceProxy mealServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMealViewModel"/> class.
        /// </summary>
        public AddMealViewModel()
        {
            this.mealServiceProxy = new MealServiceProxy();
            this.Ingredients = new ObservableCollection<IngredientModel>();
        }

        /// <summary>
        /// Gets the validation message for display in the UI.
        /// </summary>
        public string ValidationMessage { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier for the meal being created.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the meal.
        /// <summary/>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the meal.
        /// <summary/>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the image url of the meal.
        /// <summary/>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the cooking level of the meal.
        /// <summary/>
        public string CookingLevel { get; set; }

        /// <summary>
        /// Gets or sets the cooking time in minutes of the meal.
        /// <summary/>
        public string CookingTimeMins { get; set; }

        /// <summary>
        /// Gets or sets the directions of the meal.
        /// <summary/>
        public string Directions { get; set; }

        /// <summary>
        /// Gets or sets the total calories of the meal.
        /// </summary>
        public string Calories { get; set; }

        /// <summary>
        /// Gets or sets the total proteins in grams of the meal.
        /// </summary>
        public string Proteins { get; set; }

        /// <summary>
        /// Gets or sets the total carbohydrates in grams of the meal.
        /// </summary>
        public string Carbohydrates { get; set; }

        /// <summary>
        /// Gets or sets the total fats in grams of the meal.
        /// </summary>
        public string Fats { get; set; }

        /// <summary>
        /// Gets or sets the ingredients of the meal.
        /// <summary/>
        public ObservableCollection<IngredientModel> Ingredients { get; set; }

        /// <summary>
        /// Gets or sets the selected ingredients of the meal.
        /// <summary/>
        public List<IngredientModel>? SelectedIngredients { get; set; }

        /// <summary>
        /// Loads the available ingredients into the Ingredients collection.
        /// </summary>
        public void LoadIngredients()
        {
            this.Ingredients.Add(new IngredientModel { Id = 1, Name = "Lettuce" });
            this.Ingredients.Add(new IngredientModel { Id = 2, Name = "Tomato" });
            this.Ingredients.Add(new IngredientModel { Id = 3, Name = "Chicken" });
            this.Ingredients.Add(new IngredientModel { Id = 4, Name = "Cheese" });
            this.Ingredients.Add(new IngredientModel { Id = 5, Name = "Croutons" });
        }

        /// <summary>
        /// Adds a new meal asynchronously using the current state of the ViewModel.
        /// </summary>
        /// <returns>True if the meal was successfully added; otherwise, false.</returns>
        public async Task<bool> AddMealAsync()
        {
            if (!this.IsValid(out string? error))
            {
                this.ValidationMessage = error;
                return false;
            }

            var newMeal = new MealModel
            {
                Id = this.Id,
                Name = this.Name,
                Type = this.Type,
                ImageUrl = this.ImageUrl,
                CookingLevel = this.CookingLevel,
                CookingTimeMins = int.Parse(this.CookingTimeMins),
                Directions = this.Directions,
                Calories = int.Parse(this.Calories),
                Proteins = double.Parse(this.Proteins),
                Carbohydrates = double.Parse(this.Carbohydrates),
                Fats = double.Parse(this.Fats),
                Ingredients = this.SelectedIngredients,
            };

            try
            {
                await this.mealServiceProxy.CreateAsync(newMeal);
                Debug.WriteLine($"[AddMealViewModel] Meal created with ID: {newMeal.Id}");
                return true;
            }
            catch (Exception ex)
            {
                this.ValidationMessage = $"Error creating meal: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Validates the current ViewModel state for creating a meal.
        /// </summary>
        /// <param name="error">Returns the validation error message if invalid.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private bool IsValid(out string? error)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                error = "Name is required.";
                return false;
            }

            if (!decimal.TryParse(this.CookingTimeMins, out decimal parsedTime) || parsedTime < 0)
            {
                error = "Cooking time must be a valid positive number.";
                return false;
            }

            if (!int.TryParse(this.Calories, out int parsedCalories) || parsedCalories < 0)
            {
                error = "Calories must be a valid positive integer.";
                return false;
            }

            if (!double.TryParse(this.Proteins, out double parsedProteins) || parsedProteins < 0)
            {
                error = "Proteins must be a valid positive number.";
                return false;
            }

            if (!double.TryParse(this.Carbohydrates, out double parsedCarbohydrates) || parsedCarbohydrates < 0)
            {
                error = "Carbohydrates must be a valid positive number.";
                return false;
            }

            if (!double.TryParse(this.Fats, out double parsedFats) || parsedFats < 0)
            {
                error = "Fats must be a valid positive number.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.Directions))
            {
                error = "Directions are required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.Type))
            {
                error = "Type is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.ImageUrl))
            {
                error = "Image URL must be a valid absolute URL.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CookingLevel))
            {
                error = "Cooking level is required.";
                return false;
            }

            if (this.SelectedIngredients is null)
            {
                error = "Please select an ingredient.";
                return false;
            }

            if (this.SelectedIngredients.Count == 0)
            {
                error = "At least one ingredient is required.";
                return false;
            }

            error = null;
            return true;
        }
    }
}
