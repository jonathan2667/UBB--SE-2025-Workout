namespace DesktopProject.Components
{
    using DesktopProject;
    using DesktopProject.Pages;
    using DesktopProject.Windows;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using NeoIsisJob.Views;
    using NeoIsisJob.Views.Pages;

    public sealed partial class TopBar : UserControl
    {
        private Frame frame;

        public TopBar()
        {
            this.InitializeComponent();
        }

        public void SetFrame(Frame frame)
        {
            this.frame = frame;
            SetPhoto();
            SetNavigationButtons();
        }

        private async void SetPhoto()
        {
            if (AppController.CurrentUser != null && !string.IsNullOrEmpty(string.Empty))
            {
                //UserImage.Source = await AppController.DecodeBase64ToImageAsync(controller.CurrentUser.Image);
            }
        }

        private void SetNavigationButtons()
        {
            HomeButton.Click += HomeClick;
            UserButton.Click += UserClick;
            GroupsButton.Click += GroupsClick;
            CreatePostButton.Click += CreatePostButton_Click;
            SeeUsersButton.Click += SeeUsersClick;
            GoNeo.Click += GoToNeo;
        }

        private void GoToNeo(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(MainPage));
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            HomeButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            frame.Navigate(typeof(HomeScreen));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {

            HomeButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            frame.Navigate(typeof(GroupsScreen));

        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(UserPage));
        }

        private void SeeUsersClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(UserFollow));
        }

        private void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(CreatePost));
        }


        public void SetHome()
        {
            HomeButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        }
        public void SetGroups()
        {
            HomeButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        }
        public void SetCreate()
        {
            HomeButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
        }
        public void SetNone()
        {
            HomeButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        }

        public Button HomeButtonInstance => HomeButton;
        public Button GroupsButtonInstance => GroupsButton;
        public Button CreatePostButtonInstance => CreatePostButton;
        public Button UserButtonInstance => UserButton;
    }
}
