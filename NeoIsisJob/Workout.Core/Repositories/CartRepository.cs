// <copyright file="CartRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;

    /// <summary>
    /// Provides CRUD operations for cart items in the database.
    /// </summary>
    public class CartRepository : IRepository<CartItemModel>
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing the database.</param>
        public CartRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves all cart items asynchronously.
        /// </summary>
        /// <returns>A collection of cart items belonging to the current user.</returns>
        public async Task<IEnumerable<CartItemModel>> GetAllAsync()
        {
            // The current userID needs to be properly fetched.
            int customerID = 1;

            List<CartItemModel> cartItems = await this.context.CartItems
                        .Include(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                        .Include(ci => ci.User)
                        .Where(ci => ci.UserID == customerID)
                        .ToListAsync();

            return cartItems;
        }

        /// <summary>
        /// Retrieves a cart item by its ID asynchronously.
        /// </summary>
        /// <param name="cartItemID">The ID of the cart item.</param>
        /// <returns>The cart item with the specified ID, or null if not found.</returns>
        public async Task<CartItemModel?> GetByIdAsync(int cartItemID)
        {
            // The current userID needs to be properly fetched.
            int customerID = 1;

            CartItemModel? cartItem = await this.context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .Include(ci => ci.User)
                .FirstOrDefaultAsync(ci => ci.ID == cartItemID && ci.UserID == customerID);

            return cartItem;
        }

        /// <summary>
        /// Creates a new cart item asynchronously.
        /// </summary>
        /// <param name="entity">The cart item to create.</param>
        /// <returns>The created cart item.</returns>
        public async Task<CartItemModel> CreateAsync(CartItemModel entity)
        {
            await this.context.CartItems.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Updates an existing cart item asynchronously.
        /// </summary>
        /// <param name="cartItem">The cart item to update.</param>
        /// <returns>The updated cart item.</returns>
        public async Task<CartItemModel> UpdateAsync(CartItemModel cartItem)
        {
            return await Task.FromResult(cartItem);
        }

        /// <summary>
        /// Deletes a cart item asynchronously.
        /// </summary>
        /// <param name="cartItemID">The ID of the cart item to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteAsync(int cartItemID)
        {
            try
            {
                await this.context.CartItems
                                .Where(ci => ci.ID == cartItemID)
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
