using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.Services;
using Workout.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

// Register repositories
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
