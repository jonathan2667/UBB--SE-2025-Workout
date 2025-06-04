namespace DesktopProject.Components
{
    using DesktopProject.Proxies;
    using DesktopProject.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using NeoIsisJob.Proxy;

    public sealed partial class PostsFeed : UserControl
    {
        private const int PostsPerPage = 5;
        private int currentPage = 1;
        private PostViewModel postViewModel;

        public StackPanel PostsStackPanelPublic => this.PostsStackPanel;

        public PostsFeed()
        {
            this.InitializeComponent();

            var userServiceProxy = new UserServiceProxy();
            var postRepository = new PostServiceProxy();
            var groupRepository = new GroupServiceProxy(); // ar trebui sa fie Service si cu dependency injection nu cu new trebuie schimbat
            var postService = new PostServiceProxy();
            this.postViewModel = new PostViewModel(postService);
            this.LoadPosts();
            this.DisplayCurrentPage();
        }

        private void LoadPosts()
        {
            var controller = App.Services.GetService<AppController>();
            int userId;
            if (controller.CurrentUser == null)
            {
                userId = -1;
            }
            else
            {
                userId = controller.CurrentUser.ID;
            }

            this.postViewModel.PopulatePostsHomeFeed(userId);
        }

        public void DisplayCurrentPage()
        {
            this.PostsStackPanel.Children.Clear();
            int startIndex = (currentPage - 1) * PostsPerPage;
            int endIndex = startIndex + PostsPerPage;
            var allPosts = this.postViewModel.GetCurrentPosts();
            for (int i = startIndex; i < endIndex && i < allPosts.Count; i++)
            {
                this.PostsStackPanel.Children.Add(allPosts[i]);
            }
        }

        public void ClearPosts()
        {
            this.postViewModel.ClearPosts();
        }

        public void PopulatePostsByGroupId(long groupId)
        {
            this.postViewModel.PopulatePostsByGroupId(groupId);
        }

        public void PopulatePostsByUserId(int userId)
        {
            this.postViewModel.PopulatePostsByUserId(userId);
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentPage > 1)
            {
                this.currentPage--;
                this.DisplayCurrentPage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            var allPosts = this.postViewModel.GetCurrentPosts();
            if (this.currentPage * PostsPerPage < allPosts.Count)
            {
                this.currentPage++;
                this.DisplayCurrentPage();
            }
        }
    }
}
