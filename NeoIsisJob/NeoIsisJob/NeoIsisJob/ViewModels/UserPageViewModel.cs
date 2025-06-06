namespace NeoIsisJob.ViewModels
{
    using DesktopProject.Proxies;
    using global::Workout.Core.IServices;
    using NeoIsisJob.Proxy;
    using ServerLibraryProject.Interfaces;

    public class UserPageViewModel
    {
        //private IUserService userService;
        private UserServiceProxy userService;

        private string username = string.Empty;
        private string password = string.Empty;

        public UserPageViewModel(IUserService userService)
        {
            var userServiceProxy = new UserServiceProxy();
            this.userService = userServiceProxy;
        }
    }
}
