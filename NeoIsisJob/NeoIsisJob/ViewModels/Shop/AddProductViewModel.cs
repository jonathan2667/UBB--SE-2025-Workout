// <copyright file="AddProductViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Workout.Core.IServices;
using Workout.Core.Models;

namespace NeoIsisJob.ViewModels.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using NeoIsisJob.Proxy;

    /// <summary>
    /// ViewModel responsible for managing product addition operations.
    /// </summary>
    public class AddProductViewModel
    {
        private readonly IService<CategoryModel> categoryService;
        private readonly IService<ProductModel> productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductViewModel"/> class.
        /// </summary>
        public AddProductViewModel()
        {
            this.productService = new ProductServiceProxy();
            this.categoryService = new CategoryServiceProxy();
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

            // string name, decimal price, int stock, int categoryID, string description = "", string photoURL = ""
            // initial stock is hardcoded to 1
            var product = new ProductModel(name, price, 1, category.ID, description, imageUrl);
            return await this.productService.CreateAsync(product);
        }
    }
}
