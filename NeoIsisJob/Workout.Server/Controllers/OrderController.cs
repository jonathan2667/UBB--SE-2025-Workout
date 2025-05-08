// <copyright file="OrderController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
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
        private readonly OrderService orderServiceTyped;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The order service instance.</param>
        /// <exception cref="ArgumentException">Thrown if service is not of type OrderService.</exception>
        public OrderController(IService<OrderModel> orderService)
        {
            this.orderService = orderService;
            this.orderServiceTyped = orderService as OrderService
                ?? throw new ArgumentException("Service must be of type OrderService", nameof(orderService));
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        /// <remarks>GET: api/order.</remarks>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            IEnumerable<OrderModel> orders = await this.orderService.GetAllAsync();
            return this.Ok(orders);
        }

        /// <summary>
        /// Gets a specific order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>The order with the specified ID.</returns>
        /// <remarks>GET: api/order/{id}.</remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            OrderModel order = await this.orderService.GetByIdAsync(id);
            return order != null ? this.Ok(order) : this.NotFound();
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="order">The order to create.</param>
        /// <returns>The created order.</returns>
        /// <remarks>POST: api/order.</remarks>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OrderModel order)
        {
            OrderModel created = await this.orderService.CreateAsync(order);
            return this.CreatedAtAction(nameof(this.GetById), new { id = created.ID }, created);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="order">The updated order object.</param>
        /// <returns>The updated order.</returns>
        /// <remarks>PUT: api/order/{id}.</remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] OrderModel order)
        {
            if (id != order.ID)
            {
                return this.BadRequest("ID mismatch");
            }

            OrderModel updated = await this.orderService.UpdateAsync(order);
            return this.Ok(updated);
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>No content if deleted, NotFound otherwise.</returns>
        /// <remarks>DELETE: api/order/{id}.</remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await this.orderService.DeleteAsync(id);
            return deleted ? this.NoContent() : this.NotFound();
        }

        /// <summary>
        /// Creates an order from the current items in the cart.
        /// </summary>
        /// <returns>A confirmation message on success.</returns>
        /// <remarks>POST: api/order/from-cart.</remarks>
        [HttpPost("from-cart")]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            await this.orderServiceTyped.CreateOrderFromCart();
            return this.Ok(new { Message = "Order created from cart." });
        }
    }
}
