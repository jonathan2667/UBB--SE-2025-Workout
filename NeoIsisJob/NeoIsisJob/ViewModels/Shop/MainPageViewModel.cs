// <copyright file="MainPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Workout.Core.Data.Database;
    using Workout.Core.Models;
    using Workout.Core.Repository;
    using Workout.Core.Service;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// ViewModel responsible for managing the main page operations.
    /// </summary>
    public class MainPageViewModel
    {
        private readonly IService<ProductModel> productService;
        private readonly IService<CategoryModel> categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured or is null.");
            }

            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory(connectionString);
            DbService dbService = new DbService(dbConnectionFactory);
            IRepository<ProductModel> productRepository = new ProductRepository(dbService);
            IRepository<CategoryModel> categoryRepository = new CategoryRepository(dbService);
            this.productService = new ProductService(productRepository);
            this.categoryService = new CategoryService(categoryRepository);
        }

        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A collection of products.</returns>
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            IEnumerable<ProductModel> products = await this.productService.GetAllAsync();
            return products;
        }

        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A collection of categories.</returns>
        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryModel> categories = await this.categoryService.GetAllAsync();
            return categories;
        }

        /// <summary>
        /// Filters products by category asynchronously.
        /// </summary>
        /// <param name="categoryId">The category ID to filter by.</param>
        /// <returns>A collection of filtered products.</returns>
        public async Task<IEnumerable<ProductModel>> FilterProductsByCategoryAsync(int categoryId)
        {
            IEnumerable<ProductModel> products = await this.productService.GetAllAsync();
            return products.Where(p => p.Category.ID == categoryId);
        }

        /// <summary>
        /// Searches products by name asynchronously.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>A collection of matching products.</returns>
        public async Task<IEnumerable<ProductModel>> SearchProductsAsync(string searchTerm)
        {
            IEnumerable<ProductModel> products = await this.productService.GetAllAsync();
            return products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Applies filters to products asynchronously.
        /// </summary>
        /// <param name="filters">The filters to apply.</param>
        /// <returns>A collection of filtered products.</returns>
        public async Task<IEnumerable<ProductModel>> ApplyFiltersAsync(ProductFilters filters)
        {
            IEnumerable<ProductModel> products = await this.productService.GetAllAsync();
            return filters.Apply(products);
        }
    }
}
