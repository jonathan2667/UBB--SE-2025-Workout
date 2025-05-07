using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Xunit;

namespace Tests.Repo.Tests
{
    public class OrderRepositoryTestscs
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
        public async Task CreateAsync_Should_Add_Order()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            var order = new OrderModel
            {
                UserID = 1,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItemModel>()
            };

            // Act
            var result = await repository.CreateAsync(order);

            // Assert
            var orderInDb = await context.Orders.FirstOrDefaultAsync();
            Xunit.Assert.NotNull(orderInDb);
            Xunit.Assert.Equal(order.UserID, orderInDb!.UserID);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_EmptyList_ByDefault()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            // Act
            var orders = await repository.GetAllAsync();

            // Assert
            Xunit.Assert.Empty(orders);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_DefaultOrderModel()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Xunit.Assert.NotNull(result); // it always returns new OrderModel
            Xunit.Assert.IsType<OrderModel>(result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_SameEntity()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            var order = new OrderModel { ID = 1, UserID = 5, OrderDate = DateTime.UtcNow };

            // Act
            var updated = await repository.UpdateAsync(order);

            // Assert
            Xunit.Assert.Equal(order, updated);
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);

            // Act
            var deleted = await repository.DeleteAsync(1);

            // Assert
            Xunit.Assert.True(deleted);
        }
    }
}
