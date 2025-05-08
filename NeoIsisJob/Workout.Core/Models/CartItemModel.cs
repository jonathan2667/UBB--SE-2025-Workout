// <copyright file="CartItemModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Workout.Core.Models;

    /// <summary>
    /// Represents an item in the shopping cart.
    /// </summary>
    [Table("CartItem")]
    public class CartItemModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemModel"/> class.
        /// </summary>
        public CartItemModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemModel"/> class with specified user and product.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="productID">The ID of the product.</param>
        public CartItemModel(int userID, int productID)
        {
            this.UserID = userID;
            this.ProductID = productID;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the cart item.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated product.
        /// </summary>
        [Required]
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated user.
        /// </summary
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this cart item.
        /// </summary>
        [ForeignKey("ProductID")]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this cart item.
        /// </summary>
        [ForeignKey("UserID")]
        public UserModel User { get; set; }
    }
}
