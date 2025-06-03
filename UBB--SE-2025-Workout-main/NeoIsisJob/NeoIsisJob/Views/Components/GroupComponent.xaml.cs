namespace DesktopProject.Components
{
    using Microsoft.UI.Xaml.Controls;
    using ServerLibraryProject.Models;

    public sealed partial class GroupComponent : UserControl
    {
        public GroupComponent(Group group)
        {
            this.InitializeComponent();
            this.GroupName.Text = group.Name;
            this.GroupDescription.Text = group.Description;
            // Add more properties as needed
        }
    }
}
