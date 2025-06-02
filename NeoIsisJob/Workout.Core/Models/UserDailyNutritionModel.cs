// <copyright file="UserDailyNutritionModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System;

    /// <summary>
    /// Represents a user's daily nutrition summary including macronutrients and calories.
    /// </summary>
    public class UserDailyNutritionModel
    {
        /// <summary>
        /// Gets or sets the nutrition record identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this nutrition record.
        /// </summary>
        public virtual UserModel User { get; set; }

        /// <summary>
        /// Gets or sets the date for this nutrition record.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total calories consumed on this day.
        /// </summary>
        public int TotalCalories { get; set; }

        /// <summary>
        /// Gets or sets the total proteins consumed in grams.
        /// </summary>
        public double TotalProteins { get; set; }

        /// <summary>
        /// Gets or sets the total carbohydrates consumed in grams.
        /// </summary>
        public double TotalCarbohydrates { get; set; }

        /// <summary>
        /// Gets or sets the total fats consumed in grams.
        /// </summary>
        public double TotalFats { get; set; }

        /// <summary>
        /// Gets or sets the number of meals consumed on this day.
        /// </summary>
        public int MealsConsumed { get; set; }

        /// <summary>
        /// Gets or sets the water intake in milliliters for this day.
        /// </summary>
        public int WaterIntakeMl { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}