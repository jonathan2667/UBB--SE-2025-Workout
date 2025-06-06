using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ServerLibraryProject.Interfaces;
using ServerLibraryProject.Repositories;
using ServerLibraryProject.Services;
using Workout.Core.Data;
using Workout.Core.IRepositories;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Repositories;
using Workout.Core.Services;
using Workout.Core.Utils.Converters;

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
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();

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
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IReactionService, ReactionService>();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
