using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Xunit;
using Assert = Xunit.Assert;

public class WishlistRepoTests
{
    private WorkoutDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WorkoutDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WorkoutDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsWishlistItemsForUser()
    {
        using var context = GetInMemoryDbContext();
        context.Users.Add(new UserModel { ID = 1 });
        context.Categories.Add(new CategoryModel { ID = 1, Name = "Test Category" });
        context.Products.Add(new ProductModel { ID = 1, Name = "Product A", PhotoURL = "url", CategoryID = 1 });
        context.WishlistItems.AddRange(
            new WishlistItemModel { ID = 1, UserID = 1, ProductID = 1 },
            new WishlistItemModel { ID = 2, UserID = 1, ProductID = 1 });
        await context.SaveChangesAsync();

        var repo = new WishlistRepo(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.All(result, item => Assert.Equal(1, item.UserID));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectItem()
    {
        using var context = GetInMemoryDbContext();
        context.Users.Add(new UserModel { ID = 1 });
        context.Categories.Add(new CategoryModel { ID = 1, Name = "Cat", });
        context.Products.Add(new ProductModel { ID = 1, Name = "Product", PhotoURL = "url", CategoryID = 1 });
        context.WishlistItems.Add(new WishlistItemModel { ID = 10, UserID = 1, ProductID = 1 });
        await context.SaveChangesAsync();

        var repo = new WishlistRepo(context);
        var item = await repo.GetByIdAsync(10);

        Assert.NotNull(item);
        Assert.Equal(10, item!.ID);
    }

    [Fact]
    public async Task CreateAsync_AddsNewWishlistItem()
    {
        using var context = GetInMemoryDbContext();
        var repo = new WishlistRepo(context);

        var item = new WishlistItemModel { ID = 20, UserID = 1, ProductID = 1 };

        await context.Products.AddAsync(new ProductModel
        {
            ID = 1,
            Name = "Product",
            PhotoURL = "url",
            CategoryID = 1
        });
        await context.Categories.AddAsync(new CategoryModel { ID = 1, Name = "Category" });
        await context.Users.AddAsync(new UserModel { ID = 1 });

        await context.SaveChangesAsync();

        var result = await repo.CreateAsync(item);

        Assert.Equal(20, result.ID);
        Assert.Equal(1, result.UserID);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSameWishlistItem()
    {
        using var context = GetInMemoryDbContext();
        var repo = new WishlistRepo(context);

        var item = new WishlistItemModel { ID = 30, UserID = 1, ProductID = 1 };
        await context.WishlistItems.AddAsync(item);
        await context.SaveChangesAsync();

        var updated = await repo.UpdateAsync(item);

        Assert.Equal(item.ID, updated.ID);
    }

    [Fact]
    public async Task DeleteAsync_RemovesWishlistItem()
    {
        using var context = GetInMemoryDbContext();
        var repo = new WishlistRepo(context);

        var item = new WishlistItemModel { ID = 40, UserID = 1, ProductID = 1 };
        await context.WishlistItems.AddAsync(item);
        await context.SaveChangesAsync();

        var success = await repo.DeleteAsync(40);

        Assert.True(success);
        Assert.Empty(context.WishlistItems);
    }
}
