// <copyright file="OrderModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents an order in the system.
    /// </summary>
    [Table("Order")]
    public class OrderModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderModel"/> class.
        /// </summary>
        public OrderModel()
        {
            this.OrderItems = new List<OrderItemModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderModel"/> class with specified user and date.
        /// </summary>
        /// <param name="userID">The ID of the user placing the order.</param>
        /// <param name="orderDate">The date of the order.</param>
        public OrderModel(int userID, DateTime orderDate)
        {
            this.UserID = userID;
            this.OrderDate = orderDate;
            this.OrderItems = new List<OrderItemModel>();
        }

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
        [JsonIgnore]
        public UserModel? User { get; set; }

        /// <summary>
        /// Gets or sets the collection of order items associated with this order.
        /// </summary>
        [JsonIgnore]
        public ICollection<OrderItemModel>? OrderItems { get; set; }
    }
}
