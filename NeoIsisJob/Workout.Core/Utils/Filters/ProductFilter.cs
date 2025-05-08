// <copyright file="ProductFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Utils.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.Models;

    /// <summary>
    /// Represents a filter for products.
    /// </summary>
    public class ProductFilter : IFilter
    {
        private readonly WorkoutDbContext context;
        private readonly string searchTerm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFilter"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="searchTerm">The search term to filter products by.</param>
        public ProductFilter(WorkoutDbContext context, string searchTerm)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.searchTerm = searchTerm;
        }

        /// <summary>
        /// Applies the filter to a collection of products asynchronously.
        /// </summary>
        /// <param name="items">The collection of products to filter.</param>
        /// <returns>A task representing the asynchronous operation, containing the filtered collection of products.</returns>
        public async Task<IEnumerable<ProductModel>> ApplyAsync(IEnumerable<ProductModel> items)
        {
            if (string.IsNullOrWhiteSpace(this.searchTerm))
            {
                return items;
            }

            return await this.context.Products
                .Where(p => p.Name.Contains(this.searchTerm) || p.Description.Contains(this.searchTerm))
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
