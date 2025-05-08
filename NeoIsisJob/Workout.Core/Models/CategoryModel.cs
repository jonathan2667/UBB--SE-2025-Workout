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
        /// Initializes a new instance of the <see cref="CategoryModel"/> class.
        /// </summary>
        public CategoryModel()
        {
            this.Products = new List<ProductModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryModel"/> class with a specified name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        public CategoryModel(string name)
        {
            this.Name = name;
            this.Products = new List<ProductModel>();
        }

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