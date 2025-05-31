// <copyright file="UserMealLogModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System;

    /// <summary>
    /// Represents a logged meal that a user has consumed.
    /// </summary>
    public class UserMealLogModel
    {
        /// <summary>
        /// Gets or sets the meal log identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the meal identifier.
        /// </summary>
        public int MealId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the meal was consumed.
        /// </summary>
        public DateTime ConsumedAt { get; set; }

        /// <summary>
        /// Gets or sets the portion size multiplier (e.g., 0.5 for half portion, 2.0 for double portion).
        /// </summary>
        public double PortionMultiplier { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets optional notes about the meal consumption.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the meal navigation property.
        /// </summary>
        public MealModel Meal { get; set; }

        /// <summary>
        /// Gets or sets the user navigation property.
        /// </summary>
        public UserModel User { get; set; }
    }
} 