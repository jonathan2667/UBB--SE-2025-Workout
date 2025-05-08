using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Xunit;
using Assert = Xunit.Assert;

public class CategoryRepoTests
{
    private WorkoutDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WorkoutDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WorkoutDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        using var context = GetInMemoryDbContext();
        context.Categories.AddRange(
            new CategoryModel { ID = 1, Name = "Strength" },
            new CategoryModel { ID = 2, Name = "Cardio" });
        await context.SaveChangesAsync();

        var repo = new CategoryRepo(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectCategory()
    {
        using var context = GetInMemoryDbContext();
        context.Categories.Add(new CategoryModel { ID = 1, Name = "Yoga" });
        await context.SaveChangesAsync();

        var repo = new CategoryRepo(context);

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Yoga", result!.Name);
    }

    [Fact]
    public async Task CreateAsync_AddsCategory()
    {
        using var context = GetInMemoryDbContext();
        var repo = new CategoryRepo(context);

        var newCategory = new CategoryModel { ID = 10, Name = "Mobility" };

        var result = await repo.CreateAsync(newCategory);

        var dbItem = await context.Categories.FindAsync(10);
        Assert.NotNull(dbItem);
        Assert.Equal("Mobility", dbItem!.Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesCategoryName()
    {
        using var context = GetInMemoryDbContext();
        context.Categories.Add(new CategoryModel { ID = 1, Name = "Old Name" });
        await context.SaveChangesAsync();

        var repo = new CategoryRepo(context);
        var updated = new CategoryModel { ID = 1, Name = "New Name" };

        var result = await repo.UpdateAsync(updated);

        var dbItem = await context.Categories.FindAsync(1);
        Assert.Equal("New Name", dbItem!.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsWhenNotFound()
    {
        using var context = GetInMemoryDbContext();
        var repo = new CategoryRepo(context);

        var missing = new CategoryModel { ID = 999, Name = "Missing" };

        await Assert.ThrowsAsync<Exception>(() => repo.UpdateAsync(missing));
    }

    [Fact]
    public async Task DeleteAsync_RemovesCategory()
    {
        using var context = GetInMemoryDbContext();
        context.Categories.Add(new CategoryModel { ID = 1, Name = "Delete Me" });
        await context.SaveChangesAsync();

        var repo = new CategoryRepo(context);

        var result = await repo.DeleteAsync(1);

        Assert.True(result);
        Assert.Empty(context.Categories);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseWhenNotFound()
    {
        using var context = GetInMemoryDbContext();
        var repo = new CategoryRepo(context);

        var result = await repo.DeleteAsync(999);

        Assert.False(result);
    }
}
