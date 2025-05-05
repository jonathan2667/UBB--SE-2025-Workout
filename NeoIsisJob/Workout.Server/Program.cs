using Microsoft.EntityFrameworkCore;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;
using Workout.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure DbContext
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
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
