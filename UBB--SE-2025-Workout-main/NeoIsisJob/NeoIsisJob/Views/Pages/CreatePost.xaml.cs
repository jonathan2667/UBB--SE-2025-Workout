namespace DesktopProject.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using DesktopProject.Proxies;
    using DesktopProject.ViewModels;
    using global::Windows.Storage.Pickers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using ServerLibraryProject.Enums;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePost : Page
    {
        private AppController controller;
        private PostViewModel postViewModel;
        private IGroupService groupService;
        private List<Group> userGroups = new List<Group>();
        private string image = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePost"/> class.
        /// </summary>
        public CreatePost()
        {
            this.InitializeComponent();
            this.InitializeServices();
            this.TitleInput.TextChanged += this.TitleInput_TextChanged;
            this.DescriptionInput.TextChanged += this.DescriptionInput_TextChanged;
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.TopBar.SetFrame(this.Frame);
            this.TopBar.SetCreate();
            this.LoadUserGroups();
            this.InitializeVisibilityOptions();
        }

        private void InitializeServices()
        {
            this.controller = App.Services.GetService<AppController>();
            var postService = App.Services.GetService<IPostService>();
            this.postViewModel = new PostViewModel(postService);
            this.groupService = new GroupServiceProxy();
        }

        private void LoadUserGroups()
        {
            if (this.controller?.CurrentUser == null)
            {
                throw new InvalidOperationException("CurrentUser is not set in the AppController.");
            }

            this.userGroups = this.groupService.GetUserGroups(this.controller.CurrentUser.Id);
            this.GroupsListBox.ItemsSource = this.userGroups;
        }

        private void InitializeVisibilityOptions()
        {
            this.VisibilityComboBox.ItemsSource = Enum.GetValues(typeof(PostVisibility));
            this.VisibilityComboBox.SelectedIndex = 0;
        }

        private void TitleInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TitleCharCounter.Text = $"{this.TitleInput.Text.Length}/50";
        }

        private void DescriptionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.DescriptionCharCounter.Text = $"{this.DescriptionInput.Text.Length}/250";
        }

        private void VisibilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.GroupSelectionPanel.Visibility = ((PostVisibility)this.VisibilityComboBox.SelectedItem) == PostVisibility.Groups
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private async void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                };
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.CurrentWindow);
                //WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                // StorageFile file = await picker.PickSingleFileAsync();
                //if (file != null)
                //{
                //this.image = "image://" + await AppController.EncodeImageToBase64Async(file);
                this.image = "image://";
                this.DescriptionInput.Text = "";
                this.Confirmation.Text = "Image uploaded.";
                //}
            }
            catch (Exception ex)
            {
                this.ErrorMessage.Text = $"Error uploading image: {ex.Message}";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ValidateInputs();
                var selectedVisibility = (PostVisibility)this.VisibilityComboBox.SelectedItem;
                var post = this.CreateNewPost(selectedVisibility);

                this.postViewModel.AddPost(
                    post.Title,
                    post.Content,
                    post.UserId,
                    post.GroupId, // Fix: Convert nullable long to long
                    post.Visibility,
                    post.Tag);               

                this.Frame.Navigate(typeof(HomeScreen));
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(this.TitleInput.Text))
            {
                throw new Exception("Title is required!");
            }

            if (this.image == string.Empty && string.IsNullOrWhiteSpace(this.DescriptionInput.Text))
            {
                throw new Exception("Content cannot be empty!");
            }

            var selectedVisibility = (PostVisibility)this.VisibilityComboBox.SelectedItem;
            if (selectedVisibility == PostVisibility.Groups &&
               !this.GroupsListBox.SelectedItems.Any())
            {
                throw new Exception("Please select at least one group!");
            }
        }

        private Post CreateNewPost(PostVisibility visibility)
        {
            if (this.controller?.CurrentUser == null)
            {
                throw new InvalidOperationException("CurrentUser is not set in the AppController.");
            }

            var post = new Post
            {
                Title = this.TitleInput.Text.Trim(),
                Content = this.DescriptionInput.Text.Trim(),
                UserId = this.controller.CurrentUser.Id,
                CreatedDate = DateTime.Now,
                Visibility = visibility,
                Tag = this.GetSelectedTag(),
            };

            if (visibility == PostVisibility.Groups)
            {
                var selectedGroup = this.GroupsListBox.SelectedItem as Group;

                if (selectedGroup == null)
                {
                    throw new Exception("Please select a group!");
                }

                post.GroupId = selectedGroup.Id;
            }
            else
            {
                post.GroupId = null; 
            }

            return post;
        }


        private PostTag GetSelectedTag()
        {
            if (this.WorkoutRadioButton.IsChecked == true)
            {
                return PostTag.Workout;
            }

            if (this.FoodRadioButton.IsChecked == true)
            {
                return PostTag.Food;
            }

            return PostTag.Misc;
        }

        private void ShowError(string message)
        {
            this.ErrorMessage.Text = message;
            this.ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
