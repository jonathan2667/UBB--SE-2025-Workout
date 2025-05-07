// <copyright file="ProductFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.Utils.Filters
{
    /// <summary>
    /// Filter criteria for querying products.
    /// </summary>
    public class ProductFilter(int? categoryId, int? excludeProductId, int? count, string? color, string? size, string? searchTerm) : IFilter
    {
        /// <summary>
        /// Gets or sets the category ID to filter products by.
        /// </summary>
        public int? CategoryId { get; set; } = categoryId; // Initialize from constructor

        /// <summary>
        /// Gets or sets the product ID to exclude from the results.
        /// </summary>
        public int? ExcludeProductId { get; set; } = excludeProductId; // Initialize from constructor

        /// <summary>
        /// Gets or sets the maximum number of products to return.
        /// </summary>
        public int? Count { get; set; } = count; // Initialize from constructor

        /// <summary>
        /// Gets or sets the color to filter products by.
        /// </summary>
        public string? Color { get; set; } = color; // Initialize from constructor

        /// <summary>
        /// Gets or sets the size to filter products by.
        /// </summary>
        public string? Size { get; set; } = size; // Initialize from constructor

        /// <summary>
        /// Gets or sets the search term to filter products by.
        /// </summary>
        public string? SearchTerm { get; set; } = searchTerm; // Initialize from constructor
    }
}
