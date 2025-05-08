// <copyright file="WishlistViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.ViewModels.Shop
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Workout.Core.Models;
    using NeoIsisJob.Proxy;

    /// <summary>
    /// ViewModel responsible for managing wishlist operations.
    /// </summary>
    public class WishlistViewModel
    {
        private readonly WishlistServiceProxy wishlistServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistViewModel"/> class.
        /// </summary>
        public WishlistViewModel()
        {
            this.wishlistServiceProxy = new WishlistServiceProxy();
        }

        /// <summary>
        /// Retrieves all products from the wishlist asynchronously.
        /// </summary>
        /// <returns>A collection of wishlist items.</returns>
        public async Task<IEnumerable<WishlistItemModel>> GetAllProductsFromWishlistAsync()
        {
            IEnumerable<WishlistItemModel> wishlistItems = await this.wishlistServiceProxy.GetAllAsync();
            return wishlistItems;
        }

        /// <summary>
        /// Adds a product to the wishlist.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The created wishlist item.</returns>
        public async Task<WishlistItemModel> AddProductToWishlist(ProductModel product)
        {
            return await this.wishlistServiceProxy.CreateAsync(new WishlistItemModel
            {
                UserID = 1,
                ProductID = product.ID,
                Product = product,
            });
        }

        /// <summary>
        /// Removes a product from the wishlist.
        /// </summary>
        /// <param name="wishlistItemID">The ID of the wishlist item to remove.</param>
        /// <returns>True if the item was removed successfully.</returns>
        public async Task<bool> RemoveProductFromWishlist(int wishlistItemID)
        {
            return await this.wishlistServiceProxy.DeleteAsync(wishlistItemID);
        }

        /// <summary>
        /// Retrieves a product from the wishlist by product ID.
        /// </summary>
        /// <param name="productId">The product ID to find.</param>
        /// <returns>The wishlist item, or <c>null</c> if not found.</returns>
        public async Task<WishlistItemModel?> GetProductFromWishlist(int productId)
        {
            IEnumerable<WishlistItemModel> wishlistItems = await this.wishlistServiceProxy.GetAllAsync();
            foreach (WishlistItemModel item in wishlistItems)
            {
                if (item.Product.ID == productId)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
