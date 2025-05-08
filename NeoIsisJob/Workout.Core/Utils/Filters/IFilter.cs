// <copyright file="IFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Utils.Filters
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;

    /// <summary>
    /// Represents a filter interface that can be implemented to define filtering logic.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Applies the filter to a collection of items asynchronously.
        /// </summary>
        /// <param name="items">The collection of items to filter.</param>
        /// <returns>A task representing the asynchronous operation, containing the filtered collection of items.</returns>
        Task<IEnumerable<ProductModel>> ApplyAsync(IEnumerable<ProductModel> items);
    }
}
