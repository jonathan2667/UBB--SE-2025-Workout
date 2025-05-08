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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
        {
            var products = await this.productService.GetAllAsync();
            return this.Ok(products);
        }

        /// <summary>
        /// Gets a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = await this.productService.GetByIdAsync(id);
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
        [HttpPost]
        public async Task<ActionResult<ProductModel>> Create([FromBody] ProductModel model)
        {
            var created = await this.productService.CreateAsync(model);
            return this.CreatedAtAction(nameof(this.GetById), new { id = created.ID }, created);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <param name="model">The updated product model.</param>
        /// <returns>The updated product.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> Update(int id, [FromBody] ProductModel model)
        {
            if (id != model.ID)
            {
                return this.BadRequest("Product ID mismatch.");
            }

            var updated = await this.productService.UpdateAsync(model);
            return this.Ok(updated);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>No content if successful, or NotFound if not.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await this.productService.DeleteAsync(id);
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
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetFiltered([FromBody] IFilter filter)
        {
            var products = await this.productService.GetFilteredAsync(filter);
            return this.Ok(products);
        }
    }
}
