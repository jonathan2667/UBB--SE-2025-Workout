// <copyright file="CategoryFilter.cs" company="PlaceholderCompany">
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
    /// Represents a filter for filtering products by category.
    /// </summary>
    public class CategoryFilter : IFilter
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryFilter"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="categoryId">The category ID to filter by.</param>
        public CategoryFilter(WorkoutDbContext context, int categoryId)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.CategoryID = categoryId;
        }

        /// <summary>
        /// Gets or sets the category ID to filter by.
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Applies the filter to a collection of products.
        /// </summary>
        /// <param name="items">The collection of products to filter.</param>
        /// <returns>The filtered collection of products.</returns>
        public async Task<IEnumerable<ProductModel>> ApplyAsync(IEnumerable<ProductModel> items)
        {
            return await this.context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryID == this.CategoryID)
                .ToListAsync();
        }
    }
} 