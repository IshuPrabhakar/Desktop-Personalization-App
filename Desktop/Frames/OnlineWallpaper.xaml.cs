using Desktop.Helper;
using Desktop.Model;
using Desktop.ViewModel;
using Newtonsoft.Json;
using PexelsDotNetSDK.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop.Frames
{
    /// <summary>
    /// Interaction logic for OnlineWallpaper.xaml
    /// </summary>
    public partial class OnlineWallpaper : Page
    {
        public SettingHelper Helper { get; set; }
        public string DownloadedFilename { get; set; }
        public string DownloadedThumb { get; set; }
        public OnlineWallpaper()
        {
            Helper = new SettingHelper();
            InitializeComponent();
        }

        private void SetAsWallpaper(object sender, RoutedEventArgs e)
        {
            if (Helper.IsConnectedToTheInternet() == false)
            {
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.ShowNotification("Can't Search", "Ensure that you have connected to the internet");
                return;
            }

            MenuItem Selected = sender as MenuItem;
            Online SelectedWallpaper = Selected.DataContext as Online;

            //Check if File exist or if not exist then download first then set as wallpaper
            if (!File.Exists(SelectedWallpaper.original.Substring(SelectedWallpaper.original.IndexOf("pexels-"), SelectedWallpaper.original.Length - SelectedWallpaper.original.IndexOf("pexels-"))))
            {
                download(SelectedWallpaper);
            }

            try
            {
                SettingHelper Helper = new SettingHelper();
                Helper.SetWallpaper(Helper.ThumbToFileMap(DownloadedThumb));
                Properties.Settings.Default.CurrentWallpaperPath = Helper.ThumbToFileMap(DownloadedThumb);
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void DownloadImage(object sender, RoutedEventArgs e)
        {
            if (Helper.IsConnectedToTheInternet() == false)
            {
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.ShowNotification("Can't Search", "Ensure that you have connected to the internet");
                return;
            }

            MenuItem Selected = sender as MenuItem;
            Online SelectedWallpaper = Selected.DataContext as Online;

            download(SelectedWallpaper);
        }

        private async Task download(Online SelectedWallpaper)
        {
            using (WebClient client = new WebClient())
            {
                await Task.Run(() => client.DownloadFileAsync(new Uri(SelectedWallpaper.original), Helper.CreateMediaFilePath(SelectedWallpaper.original[SelectedWallpaper.original.IndexOf("pexels-")..])));
                client.DownloadFileCompleted += OnDownloadCopmlete;
            }
            DownloadedFilename = Helper.CreateMediaFilePath(SelectedWallpaper.original.Substring(SelectedWallpaper.original.IndexOf("pexels-"), SelectedWallpaper.original.Length - SelectedWallpaper.original.IndexOf("pexels-")));
        }

        private void OnDownloadCopmlete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Helper.MakeThumbNails("Local.", DownloadedFilename);
            DownloadedThumb = Helper.CreateThumbFilePath(DownloadedFilename);
        }
    }
}