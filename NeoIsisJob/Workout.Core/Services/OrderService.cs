// <copyright file="OrderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Services
{
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// Service class for handling Order-related operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </remarks>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="cartRepository">The cart item repository.</param>
    public class OrderService(IRepository<OrderModel> orderRepository, IRepository<CartItemModel> cartRepository): IService<OrderModel>
    {
        private readonly IRepository<OrderModel> orderRepository = orderRepository;
        private readonly IRepository<CartItemModel> cartRepository = cartRepository;

        /// <inheritdoc/>
        public async Task<OrderModel> CreateAsync(OrderModel entity)
        {
            return await this.orderRepository.CreateAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await this.orderRepository.DeleteAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await this.orderRepository.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<OrderModel> GetByIdAsync(int id)
        {
            return await this.orderRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public Task<OrderModel> UpdateAsync(OrderModel entity)
        {
            return this.orderRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Creates an order from the current items in the cart.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateOrderFromCart()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartRepository.GetAllAsync();

            List<OrderItemModel> orderItems = cartItems.Select(cartItem => new OrderItemModel
            {
                ProductID = cartItem.Product.ID,
                Quantity = 1,
            }).ToList();

            foreach (CartItemModel cartItem in cartItems)
            {
                await this.cartRepository.DeleteAsync(cartItem.ID);
            }

            OrderModel order = new OrderModel
            {
                UserID = cartItems.First().UserID, // Assuming all items belong to the same user
                OrderDate = DateTime.Now,
                OrderItems = orderItems,
            };

            await this.orderRepository.CreateAsync(order);
        }
    }
}
