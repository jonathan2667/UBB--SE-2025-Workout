// <copyright file="IService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.IServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Generic interface for service classes.
    /// </summary>
    /// <typeparam name="T">The entity type this service operates on.</typeparam>
    public interface IService<T>
        where T : class
    {
        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A task representing the asynchronous operation with the entity.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>A task representing the asynchronous operation with the created entity.</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation with the updated entity.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Gets a collection of products based on the provided filter criteria.
        /// This method calls the repository's filtered get method.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of products.</returns>
        // Added a new generic filtering method
        Task<IEnumerable<T>> GetFilteredAsync(IFilter filter)
        {
            return Task.FromResult(Enumerable.Empty<T>());
        }
    }
}
