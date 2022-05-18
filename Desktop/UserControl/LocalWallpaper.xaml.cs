using Desktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Desktop.Helper;
using Desktop.Frames;

namespace Desktop.UserControl
{
    /// <summary>
    /// Interaction logic for LocalWallpaper.xaml
    /// </summary>
    public partial class LocalWallpaper
    {
        //public ObservableCollection<Wallpaper> LocalWallpaper { get; set; }
        public ObservableCollection<Wallpaper> CollectionFiles { get; set; }
        public Config config { get; set; }

        public LocalWallpaper()
        {
            config = new Config();
            //LocalWallpaper = new ObservableCollection<Wallpaper>();
            CollectionFiles = new ObservableCollection<Wallpaper>();

            // Load Collection
            GetCollections();
            GetWallPapers();

            InitializeComponent();
        }

        private void GetCollections()
        {
            GetLocalWallpaper Coll = new();
            CollectionFiles = Coll.GetCollection();

        }

        private void GetWallPapers()
        {
            GetLocalWallpaper localWallpaper = new GetLocalWallpaper();
            //LocalWallpaper = localWallpaper.GetWallPaper(localWallpaper.GetFolder());
        }

        private void MoreClick(object sender, RoutedEventArgs e)
        {
            SystemWallpaperList.Visibility = Visibility.Visible;
        }

        private void SystemWallpaperList_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Selected = sender as MenuItem;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;

            SettingHelper helper = new SettingHelper();
            try
            {
                helper.SetWallpaper(helper.ThumbToFileMap(SelectedWallpaper.FilePath));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void LocalWallpaper_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Selected = sender as MenuItem;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;

            SettingHelper helper = new SettingHelper();
            try
            {
                helper.SetWallpaper(helper.ThumbToFileMap(SelectedWallpaper.FilePath));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void Add_To_Collection(object sender, RoutedEventArgs e)
        {
            SettingHelper helper = new SettingHelper();
            MenuItem Selected = sender as MenuItem;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;
            Wallpapers wallpapers = new Wallpapers();

            System.Diagnostics.Debug.WriteLine(SelectedWallpaper.FilePath);
            //if (!wallpapers.RecentFiles.Contains(SelectedWallpaper))
            //{
            //    wallpapers.RecentFiles.Add(SelectedWallpaper);
            //}
        }

        private void PlayPreview(object sender, MouseButtonEventArgs e)
        {
            SettingHelper helper = new SettingHelper();

            Button Selected = sender as Button;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;
            config.PreviewWallpaper = SelectedWallpaper.FilePath;
            //helper.Save(config, "config.secure", typeof(Config));

            //Parent.PreviewWallpaper("HI!...");
            //System.Diagnostics.Debug.WriteLine(Parent + " ");
            //Properties.Settings.Default.CurrentWallpaperPath = SelectedWallpaper.FilePath;
            //Properties.Settings.Default.Save();
        }
    }
}
