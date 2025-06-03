namespace NeoIsisJob.Services
{
    using DesktopProject.Proxies;
    using NeoIsisJob.Proxy;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class UserPageService
    {
        // private readonly IUserService userServiceProxy;
        private readonly UserServiceProxy userServiceProxy;

        public UserPageService()
        {
            userServiceProxy = new UserServiceProxy();
        }

        public long UserHasAnAccount(string name)
        {
            UserModel? user = userServiceProxy.GetUserByUsername(name);

            return user == null ? -1 : user.ID;
        }

        public long InsertNewUser(string name, string password)
        {
            UserModel user = new UserModel
            {
                Username = name,
                Password = password,
            };

            var createdUser = userServiceProxy.AddUser(user.Username, user.Password, string.Empty);

            return createdUser == null ? -1 : createdUser;
        }
    }
}