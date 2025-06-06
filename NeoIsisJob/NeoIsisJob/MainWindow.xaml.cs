using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Views;

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

            // Set window size - add 100px to default height
            var appWindow = this.AppWindow;
            if (appWindow != null)
            {
                // Set initial window size (width: 1200 + 100 = 1300, height: 800 + 100 = 900)
                appWindow.Resize(new Windows.Graphics.SizeInt32(1450, 900));
            }

            // go directly to the login page
            MainFrame.Navigate(typeof(LoginPage));
        }
    }
}
