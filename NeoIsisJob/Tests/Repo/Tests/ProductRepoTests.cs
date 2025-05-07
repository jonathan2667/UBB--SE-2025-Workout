using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories;
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
                new CategoryModel { ID = 2, Name = "Cardio" }
            );

            context.Products.AddRange(
                new ProductModel { ID = 1, Name = "Dumbbell", Price = 20, CategoryID = 1, PhotoURL = "photo1" },
                new ProductModel { ID = 2, Name = "Treadmill", Price = 200, CategoryID = 2, PhotoURL = "photo2" }
            );
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
    }
}

