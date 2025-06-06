namespace NeoIsisJob.Views.Pages
{
    using System;
    using global::Workout.Core.Models;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using NeoIsisJob.Proxy;
    using NeoIsisJob.Services;

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
            userService = repo;
        }


        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            Username = this.UsernameTextBox.Text.Trim();
            Password = this.PasswordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please enter both your name and your password.",
                    CloseButtonText = "OK",
                    XamlRoot = XamlRoot,
                };
                _ = dialog.ShowAsync();
                return;
            }

            long userId = userPageService.UserHasAnAccount(Username);

            if (userId != -1)
            {
                try
                {
                    UserModel findUser = userService.GetUserByUsername(Username);
                    if (findUser.Password.Equals(Password))
                    {
                        AppController.CurrentUser = findUser;
                        Frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "The password is incorrect",
                            CloseButtonText = "OK",
                            XamlRoot = XamlRoot,
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
                userId = userPageService.InsertNewUser(Username, Password);

                AppController.CurrentUser = userService.GetUserAsync((int)userId).Result;
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}