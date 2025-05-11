// <copyright file="ProductRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Workout.Core.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Represents a repository for managing products in the database.
    /// Implements the <see cref="IRepository{ProductModel}"/> interface.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </remarks>
    /// <param name="context">The database context.</param>
    public class ProductRepository : IRepository<ProductModel>
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ProductRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<ProductModel> CreateAsync(ProductModel entity)
        {
            await this.context.Products.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            ProductModel? productModel = await this.context.FindAsync<ProductModel>(id);
            if (productModel == null)
            {
                return false;
            }

            this.context.Products.Remove(productModel);
            int affectedRows = await this.context.SaveChangesAsync();
            return affectedRows > 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return await this.context.Products.Include(p => p.Category).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ProductModel?> GetByIdAsync(int id)
        {
            return await this.context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ID == id);
        }

        /// <inheritdoc/>
        public async Task<ProductModel> UpdateAsync(ProductModel entity)
        {
            ProductModel? product = await this.context.Products.FirstOrDefaultAsync(p => p.ID == entity.ID);
            if (product == null)
            {
                throw new ArgumentException("Product not found", nameof(entity));
            }

            this.context.Entry(product).CurrentValues.SetValues(entity);
            await this.context.SaveChangesAsync();
            await this.context.Entry(product).ReloadAsync();
            await this.context.Entry(product).Reference(p => p.Category).LoadAsync();
            return product!;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductModel>> GetAllFilteredAsync(IFilter filter)
        {
            if (filter is not ProductFilter productFilter)
            {
                throw new ArgumentException("Invalid filter type", nameof(filter));
            }

            IQueryable<ProductModel> query = this.context.Products
                .Include(p => p.Category)
                .Where(p =>
                    ((productFilter.CategoryId == null || p.CategoryID == productFilter.CategoryId) &&
                    (productFilter.ExcludeProductId == null || p.ID != productFilter.ExcludeProductId) &&
                    (productFilter.Color == null || p.Color == productFilter.Color) &&
                    (productFilter.Size == null || p.Size == productFilter.Size)) &&
                        (productFilter.SearchTerm == null || productFilter.SearchTerm == string.Empty ||
                        p.Name.Contains(productFilter.SearchTerm) ||
                        p.Description.Contains(productFilter.SearchTerm)))

                .OrderBy(p => p.ID);
            if (productFilter.Count.HasValue && productFilter.Count.Value > 0)
            {
                query = query.Take(productFilter.Count.Value);
            }

            query = query.OrderBy(p => p.ID);
            return await query.ToListAsync();
        }
    }
}
