using Desktop.Commands;
using Desktop.Helper;
using Desktop.Model;
using System;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;
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
        public ObservableCollection<Wallpaper> CollectionFiles { get; set; }
        public SettingHelper Helper { get; set; }

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

            GetLocalWallpaper Coll = new();
            CollectionFiles = Coll.GetCollection();

            Helper = new SettingHelper();

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

            // Loop Wallpaper
            LoopWallpeper();
            Thread WallpaperThread = new Thread(() => LoopWallpeper());
            WallpaperThread.Start();
            //
        }

        private void LoopWallpeper()
        {
            int loop_every = Properties.Settings.Default.LoopFrequency;
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Enabled = true;
            timer.Elapsed += OnTimerChange;
        }

        private void OnTimerChange(object sender, ElapsedEventArgs e)
        {
            Random r = new Random();
            int random = r.Next(CollectionFiles.Count);
            SetWallaper(CollectionFiles[random]);
        }


        private void SetWallaper(Wallpaper SelectedWallpaper)
        {
            try
            {
                Helper.SetWallpaper(Helper.ThumbToFileMap(SelectedWallpaper.FilePath));
                Properties.Settings.Default.CurrentWallpaperPath = Helper.ThumbToFileMap(SelectedWallpaper.FilePath);
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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
            _ = App.Current.Dispatcher.InvokeAsync(() =>
            {
                FramePage = new Uri("Frames/Wallpapers.xaml", UriKind.RelativeOrAbsolute);
            });
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
