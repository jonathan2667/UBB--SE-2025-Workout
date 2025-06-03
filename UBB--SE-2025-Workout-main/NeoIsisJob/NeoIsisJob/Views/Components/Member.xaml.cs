// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopProject.Components
{
    using DesktopProject.Pages;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using ServerLibraryProject.Models;
    using Workout.Core.Models;

    public sealed partial class Member : UserControl
    {
        private readonly UserModel member;
        private readonly AppController controller;
        private readonly Frame navigationFrame;
        private readonly long groupId;
        private readonly bool isAdmin;

        public Member(UserModel member, Frame frame, long groupId)
        {
            this.InitializeComponent();
            this.member = member;
            this.controller = App.Services.GetService<AppController>();
            this.navigationFrame = frame;
            this.groupId = groupId;
            this.isAdmin = isAdmin;

            this.MemberName.Text = member.Username;
            this.SetImage();
            if (isAdmin && member.ID != this.controller.CurrentUser.ID) // Don’t show Remove for self
                this.RemoveButton.Visibility = Visibility.Visible;

            this.PointerPressed += this.Member_Click;
        }

        private async void SetImage()
        {
            //if (!string.IsNullOrEmpty(this.member.Image))
            //  MemberImage.Source = await AppController.DecodeBase64ToImageAsync(this.member.Image);
        }

        private void Member_Click(object sender, RoutedEventArgs e)
        {
            this.navigationFrame.Navigate(typeof(UserPage), new UserPageNavigationArgs(this.controller, this.member));
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // var groupService = new GroupService(new GroupRepository(), new UserRepository());
            // groupService.RemoveMember(groupId, member.Id);
            // this.Visibility = Visibility.Collapsed; // Hide the member locally
        }
    }
}
