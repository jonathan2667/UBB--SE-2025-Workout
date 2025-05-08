// <copyright file="AddProductViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Repositories;
    using Workout.Core.Services;

    /// <summary>
    /// ViewModel responsible for managing product addition operations.
    /// </summary>
    public class AddProductViewModel
    {
        private readonly IService<ProductModel> productService;
        private readonly IService<CategoryModel> categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductViewModel"/> class.
        /// </summary>
        public AddProductViewModel()
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
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A collection of categories.</returns>
        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryModel> categories = await this.categoryService.GetAllAsync();
            return categories;
        }

        /// <summary>
        /// Adds a new product asynchronously.
        /// </summary>
        /// <param name="name">The product name.</param>
        /// <param name="description">The product description.</param>
        /// <param name="price">The product price.</param>
        /// <param name="categoryId">The category ID.</param>
        /// <param name="imageUrl">The product image URL.</param>
        /// <returns>The created product.</returns>
        public async Task<ProductModel> AddProductAsync(string name, string description, decimal price, int categoryId, string imageUrl)
        {
            var category = await this.categoryService.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {categoryId} not found.");
            }

            var product = new ProductModel(null, name, description, price, category, imageUrl);
            return await this.productService.CreateAsync(product);
        }
    }
}
