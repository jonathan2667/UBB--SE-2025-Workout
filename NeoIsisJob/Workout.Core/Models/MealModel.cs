// <copyright file="MealModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a meal including nutritional information and its ingredients.
    /// </summary>
    public class MealModel
    {
        /// <summary>
        /// Gets or sets the meal identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the meal name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the meal type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the meal.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the cooking level (e.g., easy, medium, hard).
        /// </summary>
        public string CookingLevel { get; set; }

        /// <summary>
        /// Gets or sets the cooking time in minutes.
        /// </summary>
        public int CookingTimeMins { get; set; }

        /// <summary>
        /// Gets or sets the cooking directions.
        /// </summary>
        public string Directions { get; set; }

        /// <summary>
        /// Gets or sets the total calories of the meal.
        /// </summary>
        public int Calories { get; set; }

        /// <summary>
        /// Gets or sets the total proteins in grams.
        /// </summary>
        public double Proteins { get; set; }

        /// <summary>
        /// Gets or sets the total carbohydrates in grams.
        /// </summary>
        public double Carbohydrates { get; set; }

        /// <summary>
        /// Gets or sets the total fats in grams.
        /// </summary>
        public double Fats { get; set; }

        /// <summary>
        /// Gets or sets the list of ingredients included in the meal.
        /// </summary>
        public List<IngredientModel> Ingredients { get; set; }
    }
}
