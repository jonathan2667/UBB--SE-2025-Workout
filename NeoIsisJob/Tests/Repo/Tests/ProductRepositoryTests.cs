/*using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.Utils.Filters;
using Xunit;

namespace Tests.Repo.Tests
{
    public class ProductRepositoryTests
    {
        private WorkoutDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<WorkoutDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new WorkoutDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            context.Categories.AddRange(
                new CategoryModel { ID = 1, Name = "Weights" },
                new CategoryModel { ID = 2, Name = "Cardio" });

            context.Products.AddRange(
                new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" },
                new ProductModel { ID = 2, Name = "Treadmill", Price = 200, CategoryID = 2, PhotoURL = "photo2" });
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            var products = Xunit.Assert.IsAssignableFrom<IEnumerable<ProductModel>>(result);
            Xunit.Assert.Collection(products,
                p => Xunit.Assert.Equal("Dumbbell", p.Name),
                p => Xunit.Assert.Equal("Treadmill", p.Name));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Dumbbell", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);

            // Act
            var result = await repository.GetByIdAsync(2);

            // Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new ProductRepository(context);
            var product = new ProductModel { Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" };

            // Act
            var result = await repository.CreateAsync(product);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Dumbbell", result.Name);
            Xunit.Assert.Equal(20, result.Price);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProduct_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);
            var productToUpdate = new ProductModel { ID = 1, Name = "Updated Dumbbell", Price = 25, CategoryID = 1, PhotoURL = "photo1" };

            // Act
            var result = await repository.UpdateAsync(productToUpdate);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Updated Dumbbell", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsArgumentException_WhenNotExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);
            var productToUpdate = new ProductModel { ID = 2, Name = "Updated Dumbbell", Price = 25, CategoryID = 1, PhotoURL = "photo1" };

            // Act
            var exception = await Xunit.Assert.ThrowsAsync<ArgumentException>(() => repository.UpdateAsync(productToUpdate));

            // Assert
            Xunit.Assert.Equal("Product not found (Parameter 'entity')", exception.Message);
        }
        [Fact]
        public async Task DeleteAsync_DeletesProduct_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);

            // Act
            var result = await repository.DeleteAsync(1);

            // Assert
            Xunit.Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenProductNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);

            // Act
            var result = await repository.DeleteAsync(2);

            // Assert
            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task GetAllFilteredAsync_ReturnsFilteredProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);

            // Act
            var filter = new ProductFilter(1, null, 1, null, null, null);
            var result = await repository.GetAllFilteredAsync(filter);

            // Assert
            Xunit.Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllFilteredAsync_ThrowsArgumentException_WhenFilterIsNotProductFilter()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new CategoryModel { ID = 1, Name = "Weights" });
            context.Products.Add(new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" });
            await context.SaveChangesAsync();
            var repository = new ProductRepository(context);

            // Act
            var filter = new CartItemFilter(null, null);
            var exception = await Xunit.Assert.ThrowsAsync<ArgumentException>(() => repository.GetAllFilteredAsync(filter));

            // Assert
            Xunit.Assert.Equal("Invalid filter type (Parameter 'filter')", exception.Message);
        }
    }
}

*/