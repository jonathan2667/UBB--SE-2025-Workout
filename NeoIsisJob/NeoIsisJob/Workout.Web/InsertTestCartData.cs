using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Workout.Core.Data;
using Workout.Core.Models;

namespace Workout.Web
{
    public static class InsertTestCartData
    {
        public static async Task AddTestCartDataAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();
                
                // Clear existing cart items for user 1
                var existingItems = await context.CartItems.Where(c => c.UserID == 1).ToListAsync();
                if (existingItems.Any())
                {
                    context.CartItems.RemoveRange(existingItems);
                    await context.SaveChangesAsync();
                }
                
                // Get products from database
                var products = await context.Products.ToListAsync();
                if (!products.Any())
                {
                    // No products to add to cart
                    return;
                }
                
                // Add products to cart for user 1
                var newCartItems = new[]
                {
                    new CartItemModel { UserID = 1, ProductID = products[0].ID },
                    new CartItemModel { UserID = 1, ProductID = products.Count > 1 ? products[1].ID : products[0].ID }
                };
                
                context.CartItems.AddRange(newCartItems);
                await context.SaveChangesAsync();
            }
        }
    }
} 