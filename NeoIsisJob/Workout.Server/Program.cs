using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.Services;
using Workout.Core.Data;
using Workout.Core.Models;
using Workout.Core.Utils.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
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

app.Run();
