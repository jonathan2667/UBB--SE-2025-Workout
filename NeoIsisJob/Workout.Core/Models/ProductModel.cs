// <copyright file="ProductModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Represents a product in the system.
    /// </summary>
    [Table("Product")]
    public class ProductModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity of the product.
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the category to which the product belongs.
        /// </summary>
        [Required]
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the size of the product.
        /// </summary>
        [MaxLength(50)]
        public string Size { get; set; } = "N/A";

        /// <summary>
        /// Gets or sets the color of the product.
        /// </summary>
        [MaxLength(50)]
        public string Color { get; set; } = "N/A";

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the product's photo.
        /// </summary>
        [MaxLength(255)]
        public string PhotoURL { get; set; }

        /// <summary>
        /// Gets or sets the category to which the product belongs.
        /// </summary>
        [ForeignKey("CategoryID")]
        public CategoryModel Category { get; set; }

        /// <summary>
        /// Gets or sets the collection of cart items associated with this product.
        /// </summary>
        public ICollection<CartItemModel> CartItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of wishlist items associated with this product.
        /// </summary>
        public ICollection<WishlistItemModel> WishlistItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items associated with this product.
        /// </summary>
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}