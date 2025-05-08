// <copyright file="OrderController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Services;

    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IService<OrderModel> orderService;
        private readonly ILogger<OrderController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The order service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public OrderController(IService<OrderModel> orderService, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        /// <remarks>GET: api/order.</remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetAllOrders()
        {
            try
            {
                var orders = await this.orderService.GetAllAsync();
                return this.Ok(orders);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving orders");
                return this.StatusCode(500, "An error occurred while retrieving orders");
            }
        }

        /// <summary>
        /// Gets a specific order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <remarks>GET: api/order/{id}.</remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrder(int id)
        {
            try
            {
                var order = await this.orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return this.NotFound($"Order with ID {id} not found");
                }

                return this.Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving order {Id}", id);
                return this.StatusCode(500, "An error occurred while retrieving the order");
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="order">The order to create.</param>
        /// <returns>The created order.</returns>
        /// <remarks>POST: api/order.</remarks>
        [HttpPost]
        public async Task<ActionResult<OrderModel>> AddOrder([FromBody] OrderModel order)
        {
            if (order == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var result = await this.orderService.CreateAsync(order);
                return this.CreatedAtAction(nameof(this.GetOrder), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding order");
                return this.StatusCode(500, "An error occurred while adding the order");
            }
        }

        /// <summary>
        /// Creates a new order from the current cart items.
        /// </summary>
        /// <param name="userID">The ID of the user creating the order.</param>
        /// <returns>The created order.</returns>
        /// <remarks>POST: api/order/from-cart/{userID}.</remarks>
        [HttpPost("from-cart/{userID}")]
        public async Task<ActionResult<OrderModel>> CreateOrderFromCart(int userID)
        {
            try
            {
                var orderService = (OrderService)this.orderService;
                var result = await orderService.CreateOrderFromCart(userID);
                return this.CreatedAtAction(nameof(this.GetOrder), new { id = result.ID }, result);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating order from cart for user {UserId}", userID);
                return this.StatusCode(500, "An error occurred while creating the order from cart");
            }
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="order">The updated order object.</param>
        /// <returns>The updated order.</returns>
        /// <remarks>PUT: api/order/{id}.</remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderModel>> UpdateOrder(int id, [FromBody] OrderModel order)
        {
            if (order == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                order.ID = id;
                var result = await this.orderService.UpdateAsync(order);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating order {Id}", id);
                return this.StatusCode(500, "An error occurred while updating the order");
            }
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>No content if deleted, NotFound otherwise.</returns>
        /// <remarks>DELETE: api/order/{id}.</remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            try
            {
                var result = await this.orderService.DeleteAsync(id);
                if (!result)
                {
                    return this.NotFound($"Order with ID {id} not found");
                }

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error removing order {Id}", id);
                return this.StatusCode(500, "An error occurred while removing the order");
            }
        }
    }
}