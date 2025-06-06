// <copyright file="CategoryService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Service class for managing category operations.
    /// Implements the <see cref="IService{CategoryModel}"/> interface.
    /// </summary>
    public class CategoryService : IService<CategoryModel>
    {
        private readonly IRepository<CategoryModel> categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryService(IRepository<CategoryModel> categoryRepository)
        {
            this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        /// <summary>
        /// Gets all categories asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a collection of categories.</returns>
        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            try
            {
                var result = await this.categoryRepository.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve categories.", ex);
            }
        }

        /// <summary>
        /// Gets a category by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>A task representing the asynchronous operation with the category.</returns>
        public async Task<CategoryModel?> GetByIdAsync(int id)
        {
            try
            {
                return await this.categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve category with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="entity">The category to create.</param>
        /// <returns>A task representing the asynchronous operation with the created category.</returns>
        public async Task<CategoryModel> CreateAsync(CategoryModel entity)
        {
            try
            {
                return await this.categoryRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create category {entity.Name}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing category asynchronously.
        /// </summary>
        /// <param name="entity">The category to update.</param>
        /// <returns>A task representing the asynchronous operation with the updated category.</returns>
        public async Task<CategoryModel> UpdateAsync(CategoryModel entity)
        {
            try
            {
                return await this.categoryRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update category {entity.Name}.", ex);
            }
        }

        /// <summary>
        /// Deletes a category by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await this.categoryRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete category with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Gets a collection of categories based on the provided filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <returns>A task representing the asynchronous operation with a collection of categories.</returns>
        public Task<IEnumerable<CategoryModel>> GetFilteredAsync(IFilter filter)
        {
            return Task.FromResult(Enumerable.Empty<CategoryModel>());
        }
    }
}
