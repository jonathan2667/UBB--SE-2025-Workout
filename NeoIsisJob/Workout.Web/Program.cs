using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Workout.Web.Data;
using Workout.Core.Data;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.Services;
using Workout.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// Allow top-level statements to use await
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add WorkoutDbContext
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IUserClassRepo, UserClassRepo>();
builder.Services.AddScoped<IRepository<CartItemModel>, CartRepository>();
builder.Services.AddScoped<IRepository<WishlistItemModel>, WishlistRepo>();

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IUserClassService, UserClassService>();
builder.Services.AddScoped<IService<CartItemModel>, CartService>();
builder.Services.AddScoped<IService<WishlistItemModel>, WishlistService>();

// Configure HttpClient for API
builder.Services.AddHttpClient("API", client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5111/api";
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    
    // Initialize test data in development environment
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            // Test data initialization removed
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while initializing test data.");
        }
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
