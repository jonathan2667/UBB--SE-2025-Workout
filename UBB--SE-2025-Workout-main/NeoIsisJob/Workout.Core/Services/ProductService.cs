// <copyright file="ProductService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Services
{
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Service class for managing products.
    /// </summary>
    public class ProductService : IService<ProductModel>
    {
        private readonly IRepository<ProductModel> productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The product repository.</param>
        public ProductService(IRepository<ProductModel> productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <inheritdoc/>
        public async Task<ProductModel> CreateAsync(ProductModel entity)
        {
            return await this.productRepository.CreateAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            return await this.productRepository.DeleteAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return await this.productRepository.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<ProductModel> GetByIdAsync(int id)
        {
            return await this.productRepository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<ProductModel> UpdateAsync(ProductModel entity)
        {
            return await this.productRepository.UpdateAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductModel>> GetFilteredAsync(IFilter filter)
        {
            return await this.productRepository.GetAllFilteredAsync(filter);
        }
    }
}
