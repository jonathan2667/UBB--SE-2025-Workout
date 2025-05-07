// <copyright file="WishlistService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Provides services for managing the wishlist, including adding, removing, and retrieving wishlist items.
    /// </summary>
    public class WishlistService : IService<WishlistItemModel>
    {
        private readonly IRepository<WishlistItemModel> wishlistRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistService"/> class.
        /// </summary>
        /// <param name="wishlistRepository">The repository for managing wishlist items.</param>
        public WishlistService(IRepository<WishlistItemModel> wishlistRepository)
        {
            this.wishlistRepository = wishlistRepository;
        }

        /// <summary>
        /// Retrieves all items in the wishlist.
        /// </summary>
        /// <returns>A list of <see cref="WishlistItemModel"/> objects representing the items in the wishlist.</returns>
        public async Task<IEnumerable<WishlistItemModel>> GetAllAsync()
        {
            try
            {
                return await this.wishlistRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve wishlist items.", ex);
            }
        }

        /// <summary>
        /// Retrieves a specific wishlist item by its ID.
        /// </summary>
        /// <param name="id">The ID of the wishlist item to retrieve.</param>
        /// <returns>The <see cref="WishlistItem"/> with the specified ID.</returns>
        public async Task<WishlistItemModel> GetByIdAsync(int id)
        {
            try
            {
                WishlistItemModel item = await this.wishlistRepository.GetByIdAsync(id)
                                        ?? throw new KeyNotFoundException($"Wishlist item with ID {id} not found.");
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve wishlist item with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Removes a specific wishlist item.
        /// </summary>
        /// <param name="wishlistItemID">The ID of the wishlist item to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<bool> DeleteAsync(int wishlistItemID)
        {
            try
            {
                return await this.wishlistRepository.DeleteAsync(wishlistItemID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to remove wishlist item with ID: {wishlistItemID}.", ex);
            }
        }

        /// <summary>
        /// Adds a product to the wishlist.
        /// </summary>
        /// <param name="wishlistItem">The wishlist item to add, including product details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<WishlistItemModel> CreateAsync(WishlistItemModel wishlistItem)
        {
            try
            {
                return await this.wishlistRepository.CreateAsync(wishlistItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add product {wishlistItem.Product.ID} to wishlist.", ex);
            }
        }

        /// <summary>
        /// Clears all items from the wishlist.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ResetWishlist()
        {
            try
            {
                IEnumerable<WishlistItemModel> wishlistItems = await this.wishlistRepository.GetAllAsync();
                foreach (WishlistItemModel item in wishlistItems)
                {
                    await this.wishlistRepository.DeleteAsync(item.ID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to reset wishlist.", ex);
            }
        }

        /// <summary>
        /// This method is not implemented as the wishlist service does not support updating wishlist items directly.
        /// </summary>
        /// <param name="entity">The wishlist item to update, including updated product details and quantity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation, with the updated <see cref="WishlistItem"/> result.</returns>
        public Task<WishlistItemModel> UpdateAsync(WishlistItemModel entity)
        {
            return Task.FromResult(entity);
        }

        /// <summary>
        /// This method is not implemented as the wishlist service does not support filtering wishlist items directly.
        /// </summary>
        /// <param name="filter">The filter criteria to apply to the wishlist items.</param>
        /// <returns>A list of <see cref="WishlistItem"/> objects that match the filter criteria.</returns>
        public Task<IEnumerable<WishlistItemModel>> GetFilteredAsync(IFilter filter)
        {
            return Task.FromResult<IEnumerable<WishlistItemModel>>(new List<WishlistItemModel>());
        }
    }
}
