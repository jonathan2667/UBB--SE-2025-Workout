
namespace NeoIsisJob
{
    using System;
    using ServerLibraryProject.Models;
    using Workout.Core.Models;

    public sealed class AppController
    {
        private static readonly Lazy<AppController> instance = new(() => new AppController());

        public static AppController Instance => instance.Value;

        public UserModel? CurrentUser { get; set; }

        public AppController() { }
    }
}