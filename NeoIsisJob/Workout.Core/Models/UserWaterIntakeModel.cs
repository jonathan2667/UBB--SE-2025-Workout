// <copyright file="UserWaterIntakeModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System;

    /// <summary>
    /// Represents a single water intake entry for a user.
    /// </summary>
    public class UserWaterIntakeModel
    {
        /// <summary>
        /// Gets or sets the water intake record identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this water intake record.
        /// </summary>
        public virtual UserModel User { get; set; }

        /// <summary>
        /// Gets or sets the amount of water consumed in milliliters.
        /// </summary>
        public int AmountMl { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the water was consumed.
        /// </summary>
        public DateTime ConsumedAt { get; set; }

        /// <summary>
        /// Gets or sets optional notes about the water intake (e.g., "with meal", "during workout").
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
} 