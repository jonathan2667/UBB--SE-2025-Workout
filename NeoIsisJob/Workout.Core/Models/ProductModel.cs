// <copyright file="ProductModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a product in the system.
    /// </summary>
    [Table("Product")]
    public class ProductModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductModel"/> class.
        /// </summary>
        public ProductModel()
        {
            this.CartItems = new List<CartItemModel>();
            this.WishlistItems = new List<WishlistItemModel>();
            this.OrderItems = new List<OrderItemModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductModel"/> class with specified properties.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="price">The price of the product.</param>
        /// <param name="stock">The stock quantity of the product.</param>
        /// <param name="categoryID">The ID of the category to which the product belongs.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="photoURL">The URL of the product's photo.</param>
        public ProductModel(string name, decimal price, int stock, int categoryID, string description = "", string photoURL = "")
        {
            this.Name = name;
            this.Price = price;
            this.Stock = stock;
            this.CategoryID = categoryID;
            this.Description = description;
            this.PhotoURL = photoURL;
            this.CartItems = new List<CartItemModel>();
            this.WishlistItems = new List<WishlistItemModel>();
            this.OrderItems = new List<OrderItemModel>();
        }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Required]
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
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product's photo.
        /// </summary>
        [StringLength(500)]
        public string? PhotoURL { get; set; }

        /// <summary>
        /// Gets or sets the category to which the product belongs.
        /// </summary>
        [ForeignKey("CategoryID")]
        [JsonIgnore]
        public CategoryModel? Category { get; set; }

        /// <summary>
        /// Gets or sets the collection of cart items associated with this product.
        /// </summary>
        [JsonIgnore]
        public ICollection<CartItemModel>? CartItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of wishlist items associated with this product.
        /// </summary>
        [JsonIgnore]
        public ICollection<WishlistItemModel>? WishlistItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items associated with this product.
        /// </summary>
        [JsonIgnore]
        public ICollection<OrderItemModel>? OrderItems { get; set; }
    }
}