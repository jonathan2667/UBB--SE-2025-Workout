// <copyright file="OrderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// Service class for handling Order-related operations.
    /// </summary>
    public class OrderService : IService<OrderModel>
    {
        private readonly IRepository<OrderModel> orderRepository;
        private readonly IRepository<CartItemModel> cartRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        /// <param name="cartRepository">The cart item repository.</param>
        public OrderService(IRepository<OrderModel> orderRepository, IRepository<CartItemModel> cartRepository)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        /// <inheritdoc/>
        public async Task<OrderModel> CreateAsync(OrderModel entity)
        {
            try
            {
                return await this.orderRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create order.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await this.orderRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete order with ID {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            try
            {
                return await this.orderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve orders.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<OrderModel> GetByIdAsync(int id)
        {
            return await this.orderRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<OrderModel> UpdateAsync(OrderModel entity)
        {
            try
            {
                return await this.orderRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update order with ID {entity.ID}.", ex);
            }
        }

        /// <summary>
        /// Creates an order from the current items in the cart.
        /// </summary>
        /// <param name="userID">The ID of the user creating the order.</param>
        /// <returns>The created order.</returns>
        public async Task<OrderModel> CreateOrderFromCart(int userID)
        {
            try
            {
                var cartItems = await this.cartRepository.GetAllAsync();
                var userCartItems = cartItems.Where(ci => ci.UserID == userID).ToList();

                if (!userCartItems.Any())
                {
                    throw new InvalidOperationException("Cart is empty.");
                }

                var orderItems = userCartItems.Select(cartItem => new OrderItemModel
                {
                    ProductID = cartItem.ProductID,
                    Quantity = 1,
                }).ToList();

                var order = new OrderModel
                {
                    UserID = userID,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = orderItems
                };

                var createdOrder = await this.orderRepository.CreateAsync(order);

                // Clear the cart after successful order creation
                foreach (var cartItem in userCartItems)
                {
                    await this.cartRepository.DeleteAsync(cartItem.ID);
                }

                return createdOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create order from cart.", ex);
            }
        }
    }
}
