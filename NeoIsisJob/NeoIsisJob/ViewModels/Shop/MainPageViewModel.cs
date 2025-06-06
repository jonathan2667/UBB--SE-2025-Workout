﻿// <copyright file="MainPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Workout.Core.Models;

namespace NeoIsisJob.ViewModels.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using global::Workout.Core.Utils.Filters;
    using NeoIsisJob.Proxy;

    /// <summary>
    /// The view model for the main page, responsible for loading and providing product data.
    /// </summary>
    public class MainPageViewModel
    {
        private readonly ProductServiceProxy productService;
        private ProductFilter filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// Sets up the database connection and product service.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the database connection string is missing or empty.
        /// </exception>
        public MainPageViewModel()
        {
            this.productService = new ProductServiceProxy();
            this.filter = new ProductFilter(null, null, null, null, null, null);
        }

        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of products.</returns>
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await this.productService.GetFilteredAsync(this.filter);
        }

        /// <summary>
        /// Sets the selected category ID for filtering products.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        public void SetSelectedCategoryID(int categoryId)
        {
            this.filter.CategoryId = categoryId;
        }

        /// <summary>
        /// Sets the selected brand for filtering products.
        /// </summary>
        /// <param name="color">The color to be set.</param>
        public void SetSelectedColor(string color)
        {
            this.filter.Color = color;
        }

        /// <summary>
        /// Sets the selected brand for filtering products.
        /// </summary>
        /// <param name="size">The sizeto be set.</param>
        public void SetSelectedSize(string size)
        {
            this.filter.Size = size;
        }

        /// <summary>
        /// Sets the selected brand for filtering products.
        /// </summary>
        /// <param name="searchTerm">The search term to be set.</param>
        public void SetSearchTerm(string searchTerm)
        {
            this.filter.SearchTerm = searchTerm;
        }

        /// <summary>
        /// Resets the filters to their default values.
        /// </summary>
        public void ResetFilters()
        {
            this.filter = new ProductFilter(null, null, null, null, null, null);
        }
    }
}
