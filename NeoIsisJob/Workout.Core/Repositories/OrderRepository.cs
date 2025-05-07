// <copyright file="OrderRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;

    /// <summary>
    /// Represents a repository for managing orders in the database.
    /// </summary>
    public class OrderRepository : IRepository<OrderModel>
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public OrderRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<OrderModel> CreateAsync(OrderModel entity)
        {
            await this.context.Orders.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult(true);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await Task.FromResult(new List<OrderModel>());
        }

        /// <inheritdoc/>
        public async Task<OrderModel?> GetByIdAsync(int id)
        {
            return await Task.FromResult(new OrderModel());
        }

        /// <inheritdoc/>
        public async Task<OrderModel> UpdateAsync(OrderModel entity)
        {
            return await Task.FromResult(entity);
        }
    }
}
