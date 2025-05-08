// <copyright file="CategoryModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a category entity in the system.
    /// </summary>
    [Table("Category")]
    public class CategoryModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of products associated with the category.
        /// </summary>
        [JsonIgnore]
        public ICollection<ProductModel>? Products { get; set; }
    }
}