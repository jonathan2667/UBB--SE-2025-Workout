using Microsoft.UI.Xaml;
using NeoIsisJob.Views;

namespace NeoIsisJob
{
    public sealed partial class MainWindow : Window
    {
        // instance for singleton
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();
            Instance = this;

            // go directly to the main page
            MainFrame.Navigate(typeof(MainPage));
        }
    }
}
