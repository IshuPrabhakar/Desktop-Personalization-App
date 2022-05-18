using Desktop.Helper;
using Desktop.Model;
using Desktop.UserControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    /// Interaction logic for LocalWallpaper.xaml
    /// </summary>
    public partial class Local : Page
    {
        public ObservableCollection<Wallpaper> Recent { get; set; }
        public Local()
        {
            Recent = new ObservableCollection<Wallpaper>();
            InitializeComponent();
        }


        private void SystemWallpaperList_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Selected = sender as MenuItem;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;

            SettingHelper helper = new SettingHelper();
            try
            {
                helper.SetWallpaper(helper.ThumbToFileMap(SelectedWallpaper.FilePath));
                Properties.Settings.Default.CurrentWallpaperPath = helper.ThumbToFileMap(SelectedWallpaper.FilePath);
                Properties.Settings.Default.Save();

                Recent.Add(SelectedWallpaper);
                helper.Save(Recent, "temp.tmp", typeof(Wallpaper));
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

        private void DeleteLocalWallpaper(object sender, RoutedEventArgs e)
        {
            MenuItem Selected = sender as MenuItem;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;
            Thread ti = new Thread(() => { Delete(SelectedWallpaper.FilePath); });
            ti.Start();
        }

        private void Delete(string path)
        {
            Dispatcher.Invoke(() => {
                File.Delete(path);
            });
        }
    }
}
