using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NeoIsisJob.ViewModels.Workout;
using NeoIsisJob.ViewModels.Rankings;
using NeoIsisJob.ViewModels;
using NeoIsisJob.Proxy;
using NeoIsisJob.Configuration;
using Microsoft.Extensions.Configuration;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace NeoIsisJob
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        ///
        public static IServiceProvider Services { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.ConfigureServices();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            mWindow = new MainWindow();
            mWindow.Activate();
        }

        private void ConfigureServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            // Configure API settings
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ApiSettings:BaseUrl", "http://localhost:5261"}
                })
                .Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            // ✅ CORRECT: Register only ServiceProxy classes for API communication
            serviceCollection.AddScoped<UserFavoriteMealServiceProxy>();
            serviceCollection.AddScoped<MealAPIServiceProxy>();
            serviceCollection.AddScoped<RankingsServiceProxy>();
            serviceCollection.AddScoped<UserServiceProxy>();
            serviceCollection.AddScoped<WorkoutServiceProxy>();
            serviceCollection.AddScoped<UserWorkoutServiceProxy>();
            serviceCollection.AddScoped<WorkoutTypeServiceProxy>();
            serviceCollection.AddScoped<UserClassServiceProxy>();
            serviceCollection.AddScoped<ClassServiceProxy>();
            serviceCollection.AddScoped<ClassTypeServiceProxy>();
            serviceCollection.AddScoped<PersonalTrainerServiceProxy>();
            serviceCollection.AddScoped<MuscleGroupServiceProxy>();
            serviceCollection.AddScoped<ExerciseServiceProxy>();
            serviceCollection.AddScoped<CompleteWorkoutServiceProxy>();
            serviceCollection.AddScoped<UserNutritionServiceProxy>();
            serviceCollection.AddScoped<WaterTrackingServiceProxy>();
            serviceCollection.AddScoped<ProductServiceProxy>();
            serviceCollection.AddScoped<CategoryServiceProxy>();
            serviceCollection.AddScoped<CartServiceProxy>();
            serviceCollection.AddScoped<WishlistServiceProxy>();
            serviceCollection.AddScoped<OrderServiceProxy>();
            serviceCollection.AddScoped<CalendarServiceProxy>();

            // Configure ApiSettings options
            serviceCollection.Configure<ApiSettings>(options => 
            {
                options.BaseUrl = "http://localhost:5261";
            });

            // Register view models
            serviceCollection.AddSingleton<RankingsViewModel>();
            serviceCollection.AddSingleton<SelectedWorkoutViewModel>();
            serviceCollection.AddTransient<StatisticsViewModel>();

            Services = serviceCollection.BuildServiceProvider();
        }

        private Window? mWindow;
    }
}
