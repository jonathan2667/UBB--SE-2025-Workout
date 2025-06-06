// <copyright file="WishlistRepo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;

    /// <summary>
    /// Provides CRUD operations for wishlist items in the database.
    /// </summary>
    public class WishlistRepo : IRepository<WishlistItemModel>
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistRepo"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing the wishlist data.</param>
        public WishlistRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves all wishlist items.
        /// </summary>
        /// <returns>A collection of <see cref="WishlistItemModel"/> objects.</returns>
        public async Task<IEnumerable<WishlistItemModel>> GetAllAsync()
        {
            List<WishlistItemModel> wishlistItems = await this.context.WishlistItems
                .Include(wi => wi.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            return wishlistItems;
        }

        /// <summary>
        /// Retrieves a specific wishlist item by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the wishlist item to retrieve.</param>
        /// <returns>
        /// A <see cref="WishlistItemModel"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<WishlistItemModel?> GetByIdAsync(int id)
        {
            WishlistItemModel? wishlistItem = await this.context.WishlistItems
                .Include(wi => wi.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(wi => wi.ID == id);

            return wishlistItem;
        }

        /// <summary>
        /// Inserts a new wishlist item into the database for the current customer.
        /// </summary>
        /// <param name="entity">The <see cref="WishlistItemModel"/> to insert.</param>
        /// <returns>
        /// The same <see cref="WishlistItemModel"/> entity with the newly generated ID assigned.
        /// </returns>
        public async Task<WishlistItemModel> CreateAsync(WishlistItemModel entity)
        {
            await this.context.WishlistItems.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Placeholder for updating a wishlist item.
        /// Currently not implemented, as updates are not required.
        /// </summary>
        /// <param name="entity">The wishlist item to "update".</param>
        /// <returns>
        /// The same <see cref="WishlistItemModel"/> entity passed in.
        /// </returns>
        public async Task<WishlistItemModel> UpdateAsync(WishlistItemModel entity)
        {
            // Wishlist item update not required.
            return await Task.FromResult(entity);
        }

        /// <summary>
        /// Deletes a wishlist item from the database based on its ID.
        /// </summary>
        /// <param name="id">The ID of the wishlist item to delete.</param>
        /// <returns>
        /// <c>true</c> if the deletion was successful; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await this.context.WishlistItems.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            this.context.WishlistItems.Remove(item);
            await this.context.SaveChangesAsync();
            return true;
        }
    }
}
