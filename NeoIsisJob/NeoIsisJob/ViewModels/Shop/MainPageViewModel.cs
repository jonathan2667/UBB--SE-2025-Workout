// <copyright file="MainPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Workout.Core.Models;
using Workout.Core.Utils.Filters;

namespace NeoIsisJob.ViewModels.Shop
{
    using NeoIsisJob.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    /// <summary>
    /// ViewModel responsible for managing the main page operations.
    /// </summary>
    public class MainPageViewModel
    {
        private readonly ProductServiceProxy productServiceProxy;
        private readonly CategoryServiceProxy categoryServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            this.productServiceProxy = new ProductServiceProxy();
            this.categoryServiceProxy = new CategoryServiceProxy();
        }

        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A collection of products.</returns>
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            IEnumerable<ProductModel> products = await this.productServiceProxy.GetAllAsync();
            return products;
        }

        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A collection of categories.</returns>
        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryModel> categories = await this.categoryServiceProxy.GetAllAsync();
            return categories;
        }

        /// <summary>
        /// Filters products by category asynchronously.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by.</param>
        /// <returns>A collection of filtered products.</returns>
        public async Task<IEnumerable<ProductModel>> FilterProductsByCategoryAsync(int categoryId)
        {
            IEnumerable<ProductModel> products = await this.productServiceProxy.GetAllAsync();
            return products.Where(p => p.Category.ID == categoryId);
        }

        /// <summary>
        /// Searches products by name asynchronously.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>A collection of matching products.</returns>
        public async Task<IEnumerable<ProductModel>> SearchProductsAsync(string searchTerm)
        {
            IEnumerable<ProductModel> products = await this.productServiceProxy.GetAllAsync();
            return products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Applies filters to products asynchronously.
        /// </summary>
        /// <param name="filters">The filters to apply.</param>
        /// <returns>A collection of filtered products.</returns>
        public async Task<IEnumerable<ProductModel>> ApplyFiltersAsync(ProductFilter filters)
        {
            IEnumerable<ProductModel> products = await this.productServiceProxy.GetAllAsync();
            return await filters.ApplyAsync(products);
        }
    }
}
