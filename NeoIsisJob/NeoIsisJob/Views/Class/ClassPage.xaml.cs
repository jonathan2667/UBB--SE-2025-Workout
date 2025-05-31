using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
// using NeoIsisJob.Models;
using NeoIsisJob.ViewModels.Classes;
using NeoIsisJob.Views.Shop.Pages;
using NeoIsisJob.Views.Nutrition;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace NeoIsisJob.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassPage : Page
    {
        private ClassesViewModel classesViewModel;
        public ClassesViewModel ViewModel { get; }

        public ClassPage()
        {
            this.InitializeComponent();
            ViewModel = new ClassesViewModel();
            this.DataContext = ViewModel;
        }

    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isVisible = (bool)value;
            bool invert = parameter?.ToString()?.ToLower() == "inverse";
            if (invert)
            {
                isVisible = !isVisible;
            }
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
