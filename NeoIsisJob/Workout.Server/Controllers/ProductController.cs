// <copyright file="ProductController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// API controller for managing products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IService<ProductModel> productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">The service for managing products.</param>
        public ProductController(IService<ProductModel> productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        /// <remarks>GET: api/product.</remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
        {
            IEnumerable<ProductModel> products = await this.productService.GetAllAsync();
            return this.Ok(products);
        }

        /// <summary>
        /// Gets a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <remarks>GET: api/product/{id}.</remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            ProductModel product = await this.productService.GetByIdAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            return this.Ok(product);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="model">The product model.</param>
        /// <returns>The created product.</returns>
        /// <remarks>POST: api/product.</remarks>
        [HttpPost]
        public async Task<ActionResult<ProductModel>> Create([FromBody] ProductModel model)
        {
            ProductModel created = await this.productService.CreateAsync(model);
            return this.CreatedAtAction(nameof(this.GetById), new { id = created.ID }, created);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <param name="model">The updated product model.</param>
        /// <returns>The updated product.</returns>
        /// <remarks>PUT: api/product/{id}.</remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> Update(int id, [FromBody] ProductModel model)
        {
            if (id != model.ID)
            {
                return this.BadRequest("Product ID mismatch.");
            }

            ProductModel updated = await this.productService.UpdateAsync(model);
            return this.Ok(updated);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>No content if successful, or NotFound if not.</returns>
        /// <remarks>DELETE: api/product/{id}.</remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await this.productService.DeleteAsync(id);
            if (!success)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        /// <summary>
        /// Gets products based on a filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A filtered list of products.</returns>
        /// <remarks>POST: api/product/filter.</remarks>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetFiltered([FromBody] IFilter filter)
        {
            IEnumerable<ProductModel> products = await this.productService.GetFilteredAsync(filter);
            return this.Ok(products);
        }
    }
}
