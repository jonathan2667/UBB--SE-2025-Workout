// <copyright file="OrderModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents an order in the system.
    /// </summary>
    [Table("Order")]
    public class OrderModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who placed the order.
        /// </summary>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the date for the order.
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the user of the order.
        /// </summary>
        [ForeignKey("UserID")]
        public UserModel User { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items associated with this order.
        /// </summary>
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
