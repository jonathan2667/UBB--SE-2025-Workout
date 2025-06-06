namespace DesktopProject.Windows
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using NeoIsisJob.Views;

    public sealed partial class GroupsScreen : Page
    {

        public GroupsScreen()
        {
            this.InitializeComponent();
            GroupsDrawer.NavigationFrame = this.Frame;
            this.SetNavigation();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TopBar.SetFrame(this.Frame);
            TopBar.SetGroups();
            GroupsDrawer.NavigationFrame = this.Frame;
        }

        private void SetNavigation()
        {
            TopBar.HomeButtonInstance.Click += HomeClick;
            TopBar.UserButtonInstance.Click += UserClick;
            TopBar.GroupsButtonInstance.Click += GroupsClick;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeScreen));
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupsScreen));
        }
    }
}