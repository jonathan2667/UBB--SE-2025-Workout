// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WorkoutApp.Utils;
    using WorkoutApp.Utils.Filters;

    /// <summary>
    /// Defines the basic CRUD operations for a repository.
    /// </summary>
    /// <typeparam name="T">The type of entity the repository manages.</typeparam>
    public interface IRepository<T>
        where T : class
    {
        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the entity if found.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>A task representing the asynchronous operation, containing the created entity.</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation, containing the updated entity.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean indicating success.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Gets all entities filtered by a specific criteria asynchronously.
        /// </summary>
        /// <param name="filter">Filter used.</param>>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<T>> GetAllFilteredAsync(IFilter filter)
        {
            return Task.FromResult(Enumerable.Empty<T>());
        }
    }
}
