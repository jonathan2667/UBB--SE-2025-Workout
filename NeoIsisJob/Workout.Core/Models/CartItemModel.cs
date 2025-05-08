// <copyright file="CartItemModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;
    using Workout.Core.Models;

    /// <summary>
    /// Represents an item in the shopping cart.
    /// </summary>
    [Table("CartItem")]
    public class CartItemModel
    {
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
        /// </summary>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this cart item.
        /// </summary>
        [ForeignKey("ProductID")]
        [JsonIgnore]
        public ProductModel? Product { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this cart item.
        /// </summary>
        [ForeignKey("UserID")]
        [JsonIgnore]
        public UserModel? User { get; set; }
    }
}
