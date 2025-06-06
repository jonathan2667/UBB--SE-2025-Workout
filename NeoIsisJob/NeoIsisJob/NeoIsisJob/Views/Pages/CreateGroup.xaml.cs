// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopProject.Pages
{
    using System;
    using DesktopProject.Proxies;
    using DesktopProject.Windows;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using NeoIsisJob.Proxy;
    using Workout.Core.IServices;
    using Workout.Core.Models; // added this for groups

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateGroup : Page
    {
        private IGroupService groupService;
        //private IUserService userService;
        private UserServiceProxy userService;
        private string image = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGroup"/> class.
        /// </summary>
        public CreateGroup()
        {
            InitializeComponent();
            this.InitializeServices();
            GroupNameInput.TextChanged += GroupNameInput_TextChanged;
        }

        /// <summary>
        /// Initializes the services required for group and user management.
        /// </summary>
        private void InitializeServices()
        {

            groupService = new GroupServiceProxy();
            userService = new UserServiceProxy();
        }

        /// <summary>
        /// This method is called when the page is navigated to. It sets the frame for the top bar.
        /// </summary>
        /// <param name="e">The event data that provides information about the navigation.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TopBar.SetFrame(Frame);
        }

        /// <summary>
        /// Handles the TextChanged event for the GroupNameInput field.
        /// Updates the character counter to reflect the current length of the group name input.
        /// </summary>
        /// <param name="sender">The source of the event, typically the GroupNameInput control.</param>
        /// <param name="e">The event data that provides information about the TextChanged event.</param>
        private void GroupNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupNameCharCounter.Text = $"{GroupNameInput.Text.Length}/55";
        }


        /// <summary>
        /// Handles the TextChanged event for the GroupDescriptionInput field.
        /// Updates the character counter to reflect the current length of the group description input.
        /// </summary>
        /// <param name="sender">The source of the event, typically the GroupDescriptionInput control.</param>
        /// <param name="e">The event data that provides information about the TextChanged event.</param>
        private void GroupDescriptionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupDescriptionCharCounter.Text = $"{GroupDescriptionInput.Text.Length}/250";
        }

        /// <summary>
        /// Handles the click event for the Cancel button.
        /// Navigates back to the previous page in the frame.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Cancel button control.</param>
        /// <param name="e">The event data that provides information about the click event.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        /// <summary>
        /// Handles the click event for the Create Group button.
        /// Validates the input fields and creates a new group if valid.
        /// Navigates to the UserPage upon successful creation.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Create Group button control.</param>
        /// <param name="e">The event data that provides information about the click event.</param>
        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateInputs();

                var newGroup = new Group
                {
                    Name = GroupNameInput.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(GroupDescriptionInput.Text) ? null : GroupDescriptionInput.Text.Trim(),
                };

                groupService.AddGroup(newGroup.Name, newGroup.Description ?? "");
                Frame.Navigate(typeof(GroupsScreen));
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Validates the input fields for creating a new group.
        /// Throws an exception if any validation rule is violated.
        /// </summary>
        /// <exception cref="Exception">Thrown when the group name is empty, exceeds 55 characters, or when the group description exceeds 250 characters.</exception>
        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(GroupNameInput.Text))
                throw new Exception("Group name is required!");

            if (GroupNameInput.Text.Length > 55)
                throw new Exception("Group name cannot exceed 55 characters!");

            if (GroupDescriptionInput.Text.Length > 250)
                throw new Exception("Group description cannot exceed 250 characters!");
        }

        /// <summary>
        /// Displays an error message to the user.
        /// This method sets the text of the ErrorMessage control to the provided message
        /// and makes the ErrorMessage control visible. It is typically used to inform
        /// the user of validation errors or other issues that need attention.
        /// </summary>
        /// <param name="message">The error message to be displayed.</param>
        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the TextChanged event for the UserSearchBox field.
        /// Filters the list of users based on the current input in the search box.
        /// If the input is empty, it hides the search results.
        /// Otherwise, it displays the users that match the search query.
        /// </summary>
        /// <param name="sender">The source of the event, typically the UserSearchBox control.</param>
        /// <param name="e">The event data that provides information about the TextChanged event.</param>



        /// <summary>
        /// Handles the selection change event for the UserSearchResults list.
        /// This method is triggered when the user selects a user from the search results.
        /// It adds the selected user to the selected users list for the group creation process.
        /// </summary>
        /// <param name="sender">The source of the event, typically the UserSearchResults control.</param>


        /// <summary>
        /// Adds a user to the selected users list for group creation.
        /// This method creates a button representing the selected user,
        /// which can be clicked to remove the user from the selection.
        /// </summary>
        /// <param name="user">The user to be added to the selected users list.</param>

        /// <summary>
        /// Handles the Tapped event for the UserSearchBox control.
        /// This method is triggered when the user taps inside the search box.
        /// It makes the search results visible, allowing the user to see potential matches
        /// for their input. This enhances the user experience by providing immediate feedback
        /// on available users to add to the group.
        /// </summary>
        /// <param name="sender">The source of the event, typically the UserSearchBox control.</param>
        /// <param name="e">The event data that provides information about the Tapped event.</param>
        /// Note: This method does not seem to be necessary in the current context,

        /// <summary>
        /// Handles the Tapped event for the UserSearchResults control.
        /// This method is triggered when the user taps on a user from the search results.
        /// It adds the selected user to the selected users list for the group creation process.
        /// Optionally, it can hide the search results after selection.
        /// </summary>
        /// <param name="sender">The source of the event, typically the UserSearchResults control.</param>
        /// <param name="e">The event data that provides information about the Tapped event.</param>
        /// Note: This method does not seem to be necessary in the current context,

        /// <summary>
        /// Removes a user from the selected users list for group creation.
        /// This method searches for the button representing the specified user
        /// in the SelectedUsersPanel and removes it if found. This allows the user
        /// to deselect a user that was previously added to the group.
        /// </summary>
        /// <param name="user">The user to be removed from the selected users list.</param>


        /// <summary>
        /// Handles the click event for the Add Image button.
        /// This method opens a file picker dialog to allow the user to select an image file.
        /// If a file is selected, it encodes the image to a Base64 string and stores it in the 'image' variable.
        /// If an error occurs during the process, it displays an error message in the ErrorTextBox.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Add Image button control.</param>
        /// <param name="e">The event data that provides information about the click event.</param>

        /// <summary>
        /// Handles the click event for the Remove Image button.
        /// This method clears the currently selected image by setting the 'image' variable to an empty string.
        /// It allows the user to remove an image that was previously selected for the group.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Remove Image button control.</param>
        /// <param name="e">The event data that provides information about the click event.</param>
    }
}
