using DesktopProject.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ServerLibraryProject.Interfaces;
using System;
using System.Linq;

namespace DesktopProject.Components
{
    public sealed partial class GroupsDrawer : UserControl
    {
        private IGroupService groupService;
        private Frame navigationFrame;
        private AppController controller;
        private IUserService userService;
        public Frame NavigationFrame
        {
            get => navigationFrame;
            set => navigationFrame = value;
        }

        public GroupsDrawer()
        {
            this.InitializeComponent();
            groupService = App.Services.GetService<IGroupService>();
            controller = App.Services.GetService<AppController>();
            userService = App.Services.GetService<IUserService>();
            CreateGroupButton.Click += CreateGroup_Click;
            LoadGroups();
        }

        private void LoadGroups()
        {
            GroupsList.Children.Clear();

            var groups = groupService.GetAllGroups();
            var userGroups = groupService.GetUserGroups(controller.CurrentUser.Id);
            var userGroupIds = userGroups.Select(g => g.Id).ToHashSet(); // Fast lookup

            foreach (var group in groups)
            {
                var container = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var groupGrid = new Grid();
                groupGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Name
                groupGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) }); // Join/Exit

                // Group name
                var namePanel = new StackPanel { Orientation = Orientation.Horizontal };
                namePanel.Children.Add(new TextBlock
                {
                    Text = "★",
                    Foreground = new SolidColorBrush(Colors.Gold),
                    FontSize = 18,
                    VerticalAlignment = VerticalAlignment.Center
                });
                namePanel.Children.Add(new TextBlock
                {
                    Text = group.Name,
                    FontSize = 18,
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(5, 0, 0, 0)
                });
                Grid.SetColumn(namePanel, 0);
                groupGrid.Children.Add(namePanel);

                // Join or Exit button
                bool isInGroup = userGroupIds.Contains(group.Id);
                var groupActionButton = new Button
                {
                    Content = isInGroup ? "Exit" : "Join",
                    Background = new SolidColorBrush(isInGroup ? Colors.OrangeRed : Colors.Green),
                    Foreground = new SolidColorBrush(Colors.White),
                    Width = 70,
                    Height = 30,
                    Margin = new Thickness(5, 0, 0, 0),
                    Tag = group.Id
                };
                groupActionButton.Click += isInGroup ? ExitGroup_Click : JoinGroup_Click;

                Grid.SetColumn(groupActionButton, 1);
                groupGrid.Children.Add(groupActionButton);

                container.Children.Add(groupGrid);

                container.Children.Add(new TextBlock
                {
                    Text = group.Description,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.Gray),
                    Margin = new Thickness(23, 5, 0, 0)
                });

                GroupsList.Children.Add(container);
            }
        }


        private void JoinGroup_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            long groupId = (long)button.Tag;
            try
            {
                userService.JoinGroup(controller.CurrentUser.Id, groupId);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., show a message to the user)
                // For now, we can just log it or show a message box
                System.Diagnostics.Debug.WriteLine($"Error joining group: {ex.Message}");
                return;
            }

            LoadGroups(); // Refresh UI
        }

        private void ExitGroup_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            long groupId = (long)button.Tag;
            try
            {
                userService.ExitGroup(controller.CurrentUser.Id, groupId);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., show a message to the user)
                // For now, we can just log it or show a message box
                System.Diagnostics.Debug.WriteLine($"Error exiting group: {ex.Message}");
                return;
            }

            LoadGroups(); // Refresh UI
        }


        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            navigationFrame.Navigate(typeof(GroupPage), (long)((Button)sender).Tag);
        }

        private void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            navigationFrame.Navigate(typeof(CreateGroup));
        }
    }
}
