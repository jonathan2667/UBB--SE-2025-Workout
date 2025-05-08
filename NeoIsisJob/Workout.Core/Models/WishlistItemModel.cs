// <copyright file="WishlistItemModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Workout.Core.Models;

    /// <summary>
    /// Represents an item in a user's wishlist.
    /// </summary>
    [Table("WishlistItem")]
    public class WishlistItemModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistItemModel"/> class.
        /// </summary>
        public WishlistItemModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistItemModel"/> class with specified user and product.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="productID">The ID of the product.</param>
        public WishlistItemModel(int userID, int productID)
        {
            this.UserID = userID;
            this.ProductID = productID;
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
        public ProductModel Product { get; set; }

        /// <summary>
        /// Gets or sets the user who owns this wishlist item.
        /// </summary>
        [ForeignKey("UserID")]
        public UserModel User { get; set; }
    }
}
