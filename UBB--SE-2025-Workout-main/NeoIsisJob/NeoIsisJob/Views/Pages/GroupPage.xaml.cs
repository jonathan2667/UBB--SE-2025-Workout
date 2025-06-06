using System.Collections.Generic;
using DesktopProject.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NeoIsisJob;
using Workout.Core.IServices;
using Workout.Core.Models;


namespace DesktopProject.Pages
{
    public sealed partial class GroupPage : Page
    {
        private const Visibility collapsed = Visibility.Collapsed;
        private const Visibility visible = Visibility.Visible;
        private IUserService userService;
        private IPostService postService;
        private IGroupService groupService;
        private long GroupId;
        private Group group;

        public GroupPage()
        {
            this.InitializeComponent();
            this.Loaded += DisplayPage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is long id)
            {
                GroupId = id;
            }
            TopBar.SetFrame(this.Frame);
            TopBar.SetNone();
        }

        private void DisplayPage(object sender, RoutedEventArgs e)
        {
            userService = App.Services.GetService<IUserService>();
            groupService = App.Services.GetService<IGroupService>();
            postService = App.Services.GetService<IPostService>();

            group = groupService.GetGroupById(GroupId);

            SetContent();
            PopulateMembers();
        }

        private async void SetContent()
        {
            GroupTitle.Text = group.Name;
            GroupDescription.Text = group.Description;
            // if (!string.IsNullOrEmpty(group.Image))
            // GroupImage.Source = await AppController.DecodeBase64ToImageAsync(group.Image);
            PopulateFeed();
        }

        private void PopulateFeed()
        {
            this.PostsFeed.ClearPosts();
            this.PostsFeed.PopulatePostsByGroupId(GroupId);
            PostsFeed.Visibility = Visibility.Visible;
            PostsFeed.DisplayCurrentPage();
        }

        private void PopulateMembers()
        {
            this.MembersList.Children.Clear();
            List<UserModel> members = groupService.GetUsersFromGroup(GroupId);
            foreach (UserModel member in members)
            {
                this.MembersList.Children.Add(new Member(member, this.Frame, GroupId));
            }
        }

        private void CreatePostInGroupButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreatePost));
        }
    }
}