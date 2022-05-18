using Desktop.Commands;
using Desktop.Helper;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Forms = System.Windows.Forms;


namespace Desktop.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public Forms.NotifyIcon notifyIcon;
        readonly Thread Init;
        private Uri framePage;
        public Uri FramePage
        {
            get => framePage ?? new Uri("Frames/Wallpapers.xaml", UriKind.RelativeOrAbsolute);
            set { framePage = value; OnPropertyChanged("framePage"); }
        }

        /// <summary>
        /// Property Change Interface Implementation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        /// <summary>
        /// End
        /// </summary>

        public MainWindowViewModel()
        {
            Init = new Thread(() => InitializeFilesAndSettings());
            Init.Start();

            // COMMAND INVOKATION 
            WindowMove = new CommandBase(MoveWindow);
            Settings = new CommandBase(ToSettings);
            NavigateToWallpapers = new CommandBase(ToWallpaper);
            NavigateToIcons = new CommandBase(ToIcons);
            NavigateToFonts = new CommandBase(ToFonts);
            WindowClose = new CommandBase(WindowCloseCommand);
            WindowMinimise = new CommandBase(MinimiseWindow);
            WindowMaximise = new CommandBase(MaximiseWindow);
            // END COMMAND INVOKATION 
        }

        private void ToSettings(object obj)
        {
            FramePage = new Uri("Frames/Settings.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ToFonts(object Parameter)
        {
            FramePage = new Uri("Frames/Fonts.xaml", UriKind.RelativeOrAbsolute);
        }

        private void ToIcons(object Parameter)
        {
            FramePage = new Uri("Frames/Icons.xaml", UriKind.RelativeOrAbsolute);
        }

        private void MaximiseWindow(object obj)
        {
            MainWindow main = obj as MainWindow;
            if (main.WindowState == WindowState.Normal)
            {
                main.MaximiseButton.Kind = MaterialDesignThemes.Wpf.PackIconKind.FullscreenExit;      //Change in icon of maximise button
                main.WindowState = WindowState.Maximized;
            }
            else if (main.WindowState == WindowState.Maximized)
            {
                main.MaximiseButton.Kind = MaterialDesignThemes.Wpf.PackIconKind.Fullscreen;
                main.WindowState = WindowState.Normal;
            }
        }

        private void MinimiseWindow(object obj)
        {
            MainWindow main = obj as MainWindow;
            if (main.WindowState == WindowState.Normal)
            {
                main.WindowState = WindowState.Minimized;
            }
            else if (main.WindowState == WindowState.Minimized)
            {
                main.WindowState = WindowState.Normal;
            }
            else if (main.WindowState == WindowState.Maximized)
            {
                main.WindowState = WindowState.Normal;
            }
        }

        private void WindowCloseCommand(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ToWallpaper(object Parameter)
        {
            FramePage = new Uri("Frames/Wallpapers.xaml", UriKind.RelativeOrAbsolute);
        }

        private void MoveWindow(object obj)
        {
            MainWindow main = Application.Current.MainWindow as MainWindow;
           // main.DragMove();
            main.Move();
        }

        private void InitializeFilesAndSettings()
        {
            SettingHelper helper = new SettingHelper();
            string FilePath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media\Thumbnails").ToString();
            if (Directory.GetFiles(FilePath).Length == 0)
            {
                helper.MakeSystemWallpapersThumbnail();
                helper.MakeLocalWallpapersThumbnail();
            }
        }

        /// <summary>
        /// Commands for Window Controls
        /// </summary>
        public CommandBase WindowMove { get; set; }
        public CommandBase Settings { get; set; }
        public CommandBase NavigateToWallpapers { get; set; }
        public CommandBase NavigateToIcons { get; set; }
        public CommandBase NavigateToFonts { get; set; }
        public CommandBase WindowClose { get; set; }
        public CommandBase WindowMinimise { get; set; }
        public CommandBase WindowMaximise { get; set; }
        /// <summary>
        /// End
        /// </summary>
    }
}
