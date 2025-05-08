using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Xunit;
using Assert = Xunit.Assert;

public class CartRepositoryTests
{
    private WorkoutDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WorkoutDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WorkoutDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCartItemsForUser()
    {
        // Arrange
        using var context = GetInMemoryDbContext();

        var user = new UserModel { ID = 1 };
        var category = new CategoryModel { ID = 10, Name = "Category" };
        var product = new ProductModel
        {
            ID = 100,
            CategoryID = 10,
            Name = "Test Product",
            PhotoURL = "http://example.com/image.jpg"
        };

        context.Users.Add(user);
        context.Categories.Add(category);
        context.Products.Add(product);
        context.CartItems.AddRange(
            new CartItemModel { ID = 1, UserID = 1, ProductID = 100 },
            new CartItemModel { ID = 2, UserID = 1, ProductID = 100 });
        await context.SaveChangesAsync();

        var repo = new CartRepository(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, item => Assert.Equal(1, item.UserID));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectCartItem()
    {
        // Arrange
        using var context = GetInMemoryDbContext();

        context.Users.Add(new UserModel { ID = 1 });
        context.Categories.Add(new CategoryModel { Name = "TestCategory", ID = 10 });
        context.Products.Add(new ProductModel
        {
            ID = 100,
            CategoryID = 10,
            Name = "Test Product",
            PhotoURL = "http://example.com/image.jpg"
        });
        context.CartItems.Add(new CartItemModel { ID = 1, UserID = 1, ProductID = 100 });

        await context.SaveChangesAsync();

        var repo = new CartRepository(context);

        // Act
        var result = await repo.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.ID);
    }

    [Fact]
    public async Task CreateAsync_AddsNewItem()
    {
        // Arrange
        using var context = GetInMemoryDbContext();

        context.Users.Add(new UserModel { ID = 1 });
        context.Categories.Add(new CategoryModel { ID = 10, Name = "TestCategory" });
        context.Products.Add(new ProductModel
        {
            ID = 100,
            CategoryID = 10,
            Name = "Test Product",
            PhotoURL = "http://example.com/image.jpg"
        });
        await context.SaveChangesAsync();

        var repo = new CartRepository(context);

        var newItem = new CartItemModel
        {
            ID = 99,
            UserID = 1,
            ProductID = 100
        };

        // Act
        var result = await repo.CreateAsync(newItem);

        // Assert
        var dbItem = await context.CartItems.FindAsync(99);
        Assert.NotNull(dbItem);
        Assert.Equal(99, dbItem!.ID);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSameObject()
    {
        // Arrange
        using var context = GetInMemoryDbContext();

        context.Users.Add(new UserModel { ID = 1 });
        context.Products.Add(new ProductModel
        {
            ID = 100,
            CategoryID = 10,
            Name = "Test Product",
            PhotoURL = "http://example.com/image.jpg"
        });
        await context.SaveChangesAsync();

        var original = new CartItemModel { ID = 1, UserID = 1, ProductID = 100 };
        context.CartItems.Add(original);
        await context.SaveChangesAsync();

        var repo = new CartRepository(context);

        // Act
        var result = await repo.UpdateAsync(original);

        // Assert
        Assert.Equal(original.ID, result.ID); // since no real update logic
    }

    [Fact]
    public async Task DeleteAsync_RemovesCartItem()
    {
        // Arrange
        using var context = GetInMemoryDbContext();

        context.Users.Add(new UserModel { ID = 1 });
        context.Products.Add(new ProductModel
        {
            ID = 100,
            CategoryID = 10,
            Name = "Test Product",
            PhotoURL = "http://example.com/image.jpg"
        });
        context.CartItems.Add(new CartItemModel { ID = 10, UserID = 1, ProductID = 100 });
        await context.SaveChangesAsync();

        var repo = new CartRepository(context);

        // Act
        var deleted = await repo.DeleteAsync(10);

        // Assert
        Assert.True(deleted);
        Assert.Empty(context.CartItems);
    }
}
