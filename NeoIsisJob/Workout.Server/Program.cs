using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.Services;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Utils.Converters;
using System.Text.Json;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });*/

// Configure DbContext
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepo>();
builder.Services.AddScoped<IUserWorkoutRepository, UserWorkoutRepo>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IClassTypeRepository, ClassTypeRepository>();
builder.Services.AddScoped<IUserClassRepo, UserClassRepo>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepo>();
builder.Services.AddScoped<IMuscleGroupRepo, MuscleGroupRepo>();
builder.Services.AddScoped<IWorkoutTypeRepository, WorkoutTypeRepo>();
builder.Services.AddScoped<IPersonalTrainerRepo, PersonalTrainerRepo>();
builder.Services.AddScoped<ICompleteWorkoutRepository, CompleteWorkoutRepo>();
builder.Services.AddScoped<IRankingsRepository, RankingsRepository>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IRepository<MealModel>, MealRepository>();

// Add meal statistics and water tracking repositories
builder.Services.AddScoped<UserFavoriteMealRepository>();
builder.Services.AddScoped<IUserDailyNutritionRepository, UserDailyNutritionRepository>();
builder.Services.AddScoped<IUserWaterIntakeRepository, UserWaterIntakeRepository>();
builder.Services.AddScoped<IUserMealLogRepository, UserMealLogRepository>();

// Add corresponding services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IUserWorkoutService, UserWorkoutService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IClassTypeService, ClassTypeService>();
builder.Services.AddScoped<IUserClassService, UserClassService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IMuscleGroupService, MuscleGroupService>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
builder.Services.AddScoped<IPersonalTrainerService, PersonalTrainerService>();
builder.Services.AddScoped<ICompleteWorkoutService, CompleteWorkoutService>();
builder.Services.AddScoped<IRankingsService, RankingsService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IService<MealModel>, MealService>();

// Add meal statistics and water tracking services  
builder.Services.AddScoped<UserFavoriteMealService>();
builder.Services.AddScoped<IUserNutritionService, UserNutritionService>();
builder.Services.AddScoped<IWaterTrackingService, WaterTrackingService>();

builder.Services.AddScoped<IRepository<ProductModel>, ProductRepository>();
builder.Services.AddScoped<IRepository<CategoryModel>, CategoryRepo>();
builder.Services.AddScoped<IRepository<CartItemModel>, CartRepository>();
builder.Services.AddScoped<IRepository<WishlistItemModel>, WishlistRepo>();
builder.Services.AddScoped<IRepository<OrderModel>, OrderRepository>();

builder.Services.AddScoped<IService<ProductModel>, ProductService>();
builder.Services.AddScoped<IService<CategoryModel>, CategoryService>();
builder.Services.AddScoped<IService<CartItemModel>, CartService>();
builder.Services.AddScoped<IService<WishlistItemModel>, WishlistService>();
builder.Services.AddScoped<IService<OrderModel>, OrderService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Remove circular reference
/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new IFilterConverter());
    });*/
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new IFilterConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Auto-open Swagger UI in browser when running from terminal (Windows)
if (app.Environment.IsDevelopment())
{
    Task.Run(() =>
    {
        // Wait a moment for the server to start
        Thread.Sleep(2000);
        
        try
        {
            var url = "http://localhost:5261/swagger";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not automatically open browser: {ex.Message}");
            Console.WriteLine("You can manually navigate to: http://localhost:5261/swagger");
        }
    });
}

app.Run();
