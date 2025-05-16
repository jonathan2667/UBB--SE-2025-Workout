using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Workout.Web.Data;
using Workout.Core.Data;
using Workout.Core.IRepositories;
using Workout.Core.Repositories;
using Workout.Core.IServices;
using Workout.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Workout;Trusted_Connection=True;MultipleActiveResultSets=true";
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepo>();
builder.Services.AddScoped<IWorkoutTypeRepository, WorkoutTypeRepo>();
builder.Services.AddScoped<ICalendarRepository, CalendarRepository>();
builder.Services.AddScoped<IUserWorkoutRepository, UserWorkoutRepo>();

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IUserClassService, UserClassService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IWorkoutTypeService, WorkoutTypeService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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
