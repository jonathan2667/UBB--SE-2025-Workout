namespace DesktopProject.Services
{
    using DesktopProject.Proxies;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;

    public class UserPageService
    {
        private readonly IUserService userServiceProxy;

        public UserPageService()
        {
            this.userServiceProxy = new UserServiceProxy();
        }

        public long UserHasAnAccount(string name)
        {
            User? user = this.userServiceProxy.GetUserByUsername(name);

            return user == null ? -1 : user.Id;
        }

        public long InsertNewUser(string name, string password)
        {
            User user = new User
            {
                Username = name,
                Password = password,
            };

            var createdUser = this.userServiceProxy.AddUser(user.Username, user.Password, user.PhotoURL);

            return createdUser == null ? -1 : createdUser;
        }
    }
}