using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NeoIsisJob.ViewModels.Workout;
// using NeoIsisJob.Repositories;
// using NeoIsisJob.Services;
using NeoIsisJob.ViewModels.Rankings;
using Workout.Core.Repositories;
using Workout.Core.Services;
using Workout.Core.IRepositories;
using Workout.Core.IServices;
using NeoIsisJob.Proxy;
// using NeoIsisJob.Repositories.Interfaces;
// using NeoIsisJob.Services.Interfaces;

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

            // Register repositories
            serviceCollection.AddSingleton<IRankingsRepository, RankingsRepository>();

            // Register services
            serviceCollection.AddSingleton<IRankingsService, RankingsService>();

            // Register view models
            serviceCollection.AddSingleton<RankingsViewModel>();
            serviceCollection.AddSingleton<SelectedWorkoutViewModel>();

            Services = serviceCollection.BuildServiceProvider();
        }

        private Window? mWindow;
    }
}
