using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Views.Pages;

namespace NeoIsisJob
{
    public sealed partial class MainWindow : Window
    {
        // instance for singleton
        public static MainWindow Instance { get; private set; }

        public static Frame AppMainFrame { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();
            Instance = this;
            AppMainFrame = this.MainFrame;

            // go directly to the main page
            MainFrame.Navigate(typeof(UserPage));
        }
    }
}
