using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Workout.Core.Data;
using Workout.Core.Models;

public static class InsertTestData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new WorkoutDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<WorkoutDbContext>>());

        // Check if we have any products
        if (!context.Products.Any())
        {
            // Add some categories first
            var fitnessCategory = new CategoryModel { Name = "Fitness Equipment" };
            var clothingCategory = new CategoryModel { Name = "Athletic Clothing" };
            var supplementCategory = new CategoryModel { Name = "Supplements" };

            context.Categories.AddRange(fitnessCategory, clothingCategory, supplementCategory);
            await context.SaveChangesAsync();

            // Add some products
            var products = new[]
            {
                new ProductModel(
                    name: "Dumbbell Set (5-25kg)", 
                    price: 149.99m, 
                    stock: 25, 
                    categoryId: fitnessCategory.ID, 
                    description: "Complete set of dumbbells ranging from 5kg to 25kg", 
                    photoURL: "https://example.com/images/dumbbells.jpg"),
                
                new ProductModel(
                    name: "Premium Yoga Mat", 
                    price: 39.99m, 
                    stock: 100, 
                    categoryId: fitnessCategory.ID, 
                    description: "Non-slip, eco-friendly yoga mat", 
                    photoURL: "https://example.com/images/yogamat.jpg"),
                
                new ProductModel(
                    name: "Men's Compression Shirt", 
                    price: 29.99m, 
                    stock: 75, 
                    categoryId: clothingCategory.ID, 
                    size: "M", 
                    color: "Black", 
                    description: "Moisture-wicking compression shirt for optimal performance", 
                    photoURL: "https://example.com/images/compressionshirt.jpg"),
                
                new ProductModel(
                    name: "Women's Running Shorts", 
                    price: 24.99m, 
                    stock: 60, 
                    categoryId: clothingCategory.ID, 
                    size: "S", 
                    color: "Blue", 
                    description: "Lightweight, breathable running shorts with built-in liner", 
                    photoURL: "https://example.com/images/runningshorts.jpg"),
                
                new ProductModel(
                    name: "Whey Protein Powder (1kg)", 
                    price: 49.99m, 
                    stock: 120, 
                    categoryId: supplementCategory.ID, 
                    description: "High-quality whey protein with 25g protein per serving", 
                    photoURL: "https://example.com/images/protein.jpg")
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            Console.WriteLine("Products have been added to the database!");
        }

        // Add some products to a user's cart
        int userId = 1; // Default user ID
        
        // Clear any existing cart items for this user
        var existingCartItems = context.CartItems.Where(c => c.UserID == userId);
        context.CartItems.RemoveRange(existingCartItems);
        await context.SaveChangesAsync();
        
        // Get all product IDs
        var productIds = await context.Products.Select(p => p.ID).ToListAsync();
        
        // Add a few products to the cart
        if (productIds.Count > 0)
        {
            var cartItems = productIds.Take(3).Select(productId => new CartItemModel
            {
                ProductID = productId,
                UserID = userId
            });
            
            context.CartItems.AddRange(cartItems);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"Added {cartItems.Count()} items to the cart for user ID {userId}");
        }
    }
} 