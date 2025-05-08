// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.View
{
    using System;
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Windows.Graphics;
    using WinRT.Interop;

    /// <summary>
    /// The main window of the application that sets the frame and handles navigation.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            AppFrame = this.MainFrame;
            this.SetFixedSize(1440, 720);
            this.MainFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Gets the application's main navigation frame.
        /// </summary>
        public static Frame? AppFrame { get; private set; }

        /// <summary>
        /// Sets a fixed size for the window.
        /// </summary>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        private void SetFixedSize(int width, int height)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow? appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow is not null)
            {
                appWindow.Resize(new SizeInt32(width, height));
            }
        }
    }
}
