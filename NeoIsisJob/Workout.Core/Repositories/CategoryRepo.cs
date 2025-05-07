// <copyright file="CategoryRepo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics; // Required for Debug.WriteLine (optional, but good for errors)
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient; // Required for SqlParameter
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models; // Assuming Category model is here

    /// <summary>
    /// Repository class for managing Category items in the database.
    /// Implements the <see cref="IRepository{Category}"/> interface.
    /// </summary>
    public class CategoryRepo : IRepository<CategoryModel>
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepo"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing the data store.</param>
        public CategoryRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A collection of <see cref="CategoryModel"/> objects.</returns>
        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            List<CategoryModel> categories = await this.context.Categories.ToListAsync();
            return categories;
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to retrieve.</param>
        /// <returns>A <see cref="CategoryModel"/> object if found; otherwise, <c>null</c>.</returns>
        public async Task<CategoryModel?> GetByIdAsync(int id)
        {
            CategoryModel? category = await this.context.Categories.FindAsync(id);
            return category;
        }

        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <param name="entity">The category entity to create.</param>
        /// <returns>The created <see cref="CategoryModel"/> object.</returns>
        public async Task<CategoryModel> CreateAsync(CategoryModel entity)
        {
            await this.context.Categories.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="entity">The category entity to update.</param>
        /// <returns>The updated <see cref="CategoryModel"/> object.</returns>
        /// <exception cref="Exception">Thrown when no category is found with the specified ID.</exception>
        public async Task<CategoryModel> UpdateAsync(CategoryModel entity)
        {
            int rowsAffected = await this.context.Categories
                .Where(c => c.ID == entity.ID)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(x => x.Name, entity.Name));

            if (rowsAffected == 0)
            {
                throw new Exception($"No category found with ID {entity.ID}.");
            }

            return entity;
        }

        /// <summary>
        /// Deletes a category from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns><c>true</c> if the category was successfully deleted; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await this.context.Categories
                    .Where(c => c.ID == id)
                    .ExecuteDeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
