namespace DesktopProject.Pages
{
    using System;
    using DesktopProject;
    using DesktopProject.Proxies;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using NeoIsisJob.Proxy;
    using NeoIsisJob.Services;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public sealed partial class UserPage : Page
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        //private readonly IUserService userService;
        private readonly UserServiceProxy userService;

        private UserPageService userPageService = new UserPageService();

        public UserPage()
        {
            this.InitializeComponent();
            var repo = new UserServiceProxy();
            this.userService = repo;
        }


        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            this.Username = this.UsernameTextBox.Text.Trim();
            this.Password = this.PasswordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(this.Username) || string.IsNullOrEmpty(this.Password))
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please enter both your name and your password.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };
                _ = dialog.ShowAsync();
                return;
            }

            long userId = this.userPageService.UserHasAnAccount(this.Username);

            if (userId != -1)
            {

                try
                {

                    UserModel findUser = this.userService.GetUserByUsername(this.Username);
                    if (findUser.Password.Equals(this.Password))
                    {
                        App.Services.GetService<AppController>().CurrentUser = this.userService.GetUserAsync(findUser.ID).Result;
                        this.Frame.Navigate(typeof(HomeScreen));

                    }
                    else
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "The password is incorrect",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot,
                        };
                        _ = dialog.ShowAsync();
                        return;

                    }
                }
                catch (Exception)
                {
                    throw new Exception("User doesn't exist");

                }

                
            }
            else
            {
                userId = this.userPageService.InsertNewUser(this.Username, this.Password);

                App.Services.GetService<AppController>().CurrentUser = this.userService.GetUserAsync((int)userId).Result;
                this.Frame.Navigate(typeof(HomeScreen), this);
            }
        }
    }
}