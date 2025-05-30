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
using Workout.Core.Models;

// Allow top-level statements to use await
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Workout;Trusted_Connection=True;MultipleActiveResultSets=true";
//var connectionString = "Server=localhost\\SQLEXPRESS;Database=Workout;Trusted_Connection=True;MultipleActiveResultSets=true";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


/*var serverSettings = Path.Combine(Directory.GetCurrentDirectory(),
                                  "..",               // go up from Workout.Web's folder
                                  "Workout.Server",   // into the server project
                                  "appsettings.json");

// 2. Tell the Configuration system to load it  
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())  // keep the default base
    .AddJsonFile(serverSettings, optional: false, reloadOnChange: true);*/

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//var connectionString = "Data Source=localhost; Initial Catalog = Workout; Integrated Security = True; Trust Server Certificate=True; MultipleActiveResultSets=true";


// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Add HTTP client factory
builder.Services.AddHttpClient();

// Add WorkoutDbContext
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IUserClassRepo, UserClassRepo>();
builder.Services.AddScoped<IRepository<CartItemModel>, CartRepository>();
builder.Services.AddScoped<IRepository<WishlistItemModel>, WishlistRepo>();
builder.Services.AddScoped<IRepository<ProductModel>, ProductRepository>();
builder.Services.AddScoped<IRepository<CategoryModel>, CategoryRepo>();
builder.Services.AddScoped<IRepository<WishlistItemModel>, WishlistRepo>();
builder.Services.AddScoped<IRepository<CartItemModel>, CartRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepo>();
builder.Services.AddScoped<IWorkoutTypeRepository, WorkoutTypeRepo>();
builder.Services.AddScoped<ICompleteWorkoutRepository, CompleteWorkoutRepo>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepo>();
builder.Services.AddScoped<IUserWorkoutRepository, UserWorkoutRepo>();
builder.Services.AddScoped<IRankingsRepository, RankingsRepository>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IUserWorkoutRepository, UserWorkoutRepo>();
builder.Services.AddScoped<IRepository<MealModel>, MealRepository>();
builder.Services.AddScoped<UserFavoriteMealRepository>();
builder.Services.AddScoped<UserFavoriteMealService>();


// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IUserClassService, UserClassService>();
builder.Services.AddScoped<IService<CartItemModel>, CartService>();
builder.Services.AddScoped<IService<WishlistItemModel>, WishlistService>();
builder.Services.AddScoped<IService<ProductModel>, ProductService>();
builder.Services.AddScoped<IService<CategoryModel>, CategoryService>();
builder.Services.AddScoped<IService<WishlistItemModel>, WishlistService>();
builder.Services.AddScoped<IService<CartItemModel>, CartService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
builder.Services.AddScoped<ICompleteWorkoutService, CompleteWorkoutService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IUserWorkoutService, UserWorkoutService>();
builder.Services.AddScoped<IRankingsService, RankingsService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IService<MealModel>, MealService>();

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

// Add session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
