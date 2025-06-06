﻿// <copyright file="WishlistItemModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents an item in a user's wishlist.
    /// </summary>
    [Table("WishlistItem")]
    public class WishlistItemModel
    {
        public WishlistItemModel()
        {
        }

        public WishlistItemModel(int productId, int userId)
        {
            ProductID = productId;
            UserID = userId;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the wishlist item.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the product associated with this wishlist item.
        /// </summary>
        [Required]
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns this wishlist item.
        /// </summary>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this wishlist item.
        /// </summary>
        [ForeignKey("ProductID")]
        public ProductModel? Product { get; set; }

        /// <summary>
        /// Gets or sets the user who owns this wishlist item.
        /// </summary>
        [ForeignKey("UserID")]
        [JsonIgnore]
        public UserModel? User { get; set; }
    }
}
