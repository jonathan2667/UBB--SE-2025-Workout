// <copyright file="ProductController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">The service for managing products.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public ProductController(IService<ProductModel> productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        /// <remarks>GET: api/product.</remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts()
        {
            try
            {
                var products = await this.productService.GetAllAsync();
                return this.Ok(products);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving products");
                return this.StatusCode(500, "An error occurred while retrieving products");
            }
        }

        /// <summary>
        /// Gets a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <remarks>GET: api/product/{id}.</remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            try
            {
                var product = await this.productService.GetByIdAsync(id);
                if (product == null)
                {
                    return this.NotFound($"Product with ID {id} not found");
                }

                return this.Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving product {Id}", id);
                return this.StatusCode(500, "An error occurred while retrieving the product");
            }
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product model.</param>
        /// <returns>The created product.</returns>
        /// <remarks>POST: api/product.</remarks>
        [HttpPost]
        public async Task<ActionResult<ProductModel>> AddProduct([FromBody] ProductModel product)
        {
            if (product == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var result = await this.productService.CreateAsync(product);
                return this.CreatedAtAction(nameof(this.GetProduct), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding product");
                return this.StatusCode(500, "An error occurred while adding the product");
            }
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <param name="product">The updated product model.</param>
        /// <returns>The updated product.</returns>
        /// <remarks>PUT: api/product/{id}.</remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> UpdateProduct(int id, [FromBody] ProductModel product)
        {
            if (product == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                product.ID = id;
                var result = await this.productService.UpdateAsync(product);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating product {Id}", id);
                return this.StatusCode(500, "An error occurred while updating the product");
            }
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>No content if successful, or NotFound if not.</returns>
        /// <remarks>DELETE: api/product/{id}.</remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            try
            {
                var result = await this.productService.DeleteAsync(id);
                if (!result)
                {
                    return this.NotFound($"Product with ID {id} not found");
                }

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error removing product {Id}", id);
                return this.StatusCode(500, "An error occurred while removing the product");
            }
        }

        /// <summary>
        /// Gets products based on a filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A filtered list of products.</returns>
        /// <remarks>POST: api/product/filter.</remarks>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetFiltered([FromBody] ProductFilter filter)
        {
            try
            {
                IEnumerable<ProductModel> products = await this.productService.GetFilteredAsync(filter);
                return this.Ok(products);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error filtering products");
                return this.StatusCode(500, "An error occurred while filtering products");
            }
        }
    }
}