using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Microsoft.UI.Dispatching;

namespace NeoIsisJob.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Hide previous messages
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            SuccessMessageTextBlock.Visibility = Visibility.Collapsed;

            // Hardcoded login validation
            if ((username == "user1" && password == "password1") || 
                (username == "user2" && password == "password2"))
            {
                // Success
                SuccessMessageTextBlock.Text = $"Welcome, {username}! Redirecting to main page...";
                SuccessMessageTextBlock.Visibility = Visibility.Visible;
                
                // Clear the form
                UsernameTextBox.Text = "";
                PasswordBox.Password = "";
                
                // Redirect to main page after a short delay
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1.5);
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    // Navigate to MainPage using the static AppMainFrame
                    if (MainWindow.AppMainFrame != null)
                    {
                        MainWindow.AppMainFrame.Navigate(typeof(MainPage));
                    }
                };
                timer.Start();
            }
            else if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                // Empty fields
                ErrorMessageTextBlock.Text = "Please enter both username and password.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                // Invalid credentials
                ErrorMessageTextBlock.Text = "Invalid username or password. Please try again.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
} 