// <copyright file="OrderRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using Microsoft.EntityFrameworkCore;
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
            OrderModel? order = await this.context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            this.context.Orders.Remove(order);
            int affectedRows = await this.context.SaveChangesAsync();
            return affectedRows > 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await this.context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<OrderModel?> GetByIdAsync(int id)
        {
            return await this.context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.ID == id);
        }

        /// <inheritdoc/>
        public async Task<OrderModel> UpdateAsync(OrderModel entity)
        {
            OrderModel? order = await this.context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.ID == entity.ID);

            if (order == null)
            {
                throw new ArgumentException("Order not found", nameof(entity));
            }

            this.context.Entry(order).CurrentValues.SetValues(entity);
            await this.context.SaveChangesAsync();
            await this.context.Entry(order).ReloadAsync();
            return order;
        }
    }
}
