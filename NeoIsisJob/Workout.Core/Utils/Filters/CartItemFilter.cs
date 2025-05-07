// <copyright file="CartItemFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.Utils.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a filter interface that can be implemented to define filtering logic.
    /// </summary>
    public class CartItemFilter(int? productID, int? customerID): IFilter
    {
        /// <summary>
        /// Gets or sets product id to filter by.
        /// </summary>
        public int? ProductID { get; set; } = productID;

        /// <summary>
        /// Gets or sets customer id to filter by.
        /// </summary>
        public int? CustomerID { get; set; } = customerID;
    }
}
