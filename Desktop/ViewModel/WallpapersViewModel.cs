using Desktop.Commands;
using Desktop.FileDropHelper;
using Desktop.Helper;
using Desktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Desktop.ViewModel
{
    public class WallpapersViewModel : INotifyPropertyChanged
    {
        public SettingHelper Helper { get; set; }

        private string previewImageURI;
        public ObservableCollection<Wallpaper> RecentFiles { get; set; }
        public string PreviewImageURI { get => previewImageURI ?? Properties.Settings.Default.CurrentWallpaperPath; set { previewImageURI = value; OnPropertyChanged("previewImageURI"); } }
        private Uri framePage;
        public Uri FramePage
        {
            get => framePage ?? new Uri("Local.xaml", UriKind.RelativeOrAbsolute);
            set { framePage = value; OnPropertyChanged("framePage"); }
        }

        public WallpapersViewModel()
        {
            Helper = new SettingHelper();

            Thread t = new Thread(() => GetRecent());
            t.Start();

            RecentFiles = new ObservableCollection<Wallpaper>();
            GetRecent();

            // COMMAND INVOKATION 
            GetOnlineWallpaper = new CommandBase(GetOnline);
            GetAbstractWallpaper = new CommandBase(GetAbstract);
            GetLocalWallpaper = new CommandBase(GetLocal);
            UpdateWallpaper = new KeyBinding(Update);
            SetWallpaper = new CommandBase(SetLocal);
            // END 
        }

        private void GetAbstract(object obj)
        {

        }

        private void GetLocal(object obj)
        {
            FramePage = new Uri("Local.xaml", UriKind.RelativeOrAbsolute);
        }

        private void SetLocal(object obj)
        {
            MenuItem Selected = obj as MenuItem;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;

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

        private void Update(object obj)
        {
            PreviewImageURI = Properties.Settings.Default.CurrentWallpaperPath;
            RecentFiles.Clear();
            GetRecent();
        }

        private void GetOnline(object obj)
        {
            FramePage = new Uri("OnlineWallpaper.xaml", UriKind.RelativeOrAbsolute);
        }

        public ObservableCollection<Wallpaper> RetriveCollection(String FileName)
        {
            //TODO To Add Encryption
            // "gConfig.secure"
            SettingHelper helper = new SettingHelper();
            ObservableCollection<Wallpaper> Collection = new ObservableCollection<Wallpaper>();
            try
            {
                if (!File.Exists(helper.CreateConfigutionFilePath(FileName)))
                {
                    _ = File.Create(helper.CreateConfigutionFilePath(FileName));

                }
                if (new FileInfo(helper.CreateConfigutionFilePath(FileName)).Length != 0)
                {
                    using (StreamReader sr = new StreamReader(helper.CreateConfigutionFilePath(FileName)))
                    {
                        Collection = JsonConvert.DeserializeObject<ObservableCollection<Wallpaper>>(sr.ReadToEnd());
                    }

                }

            }
            catch (Exception e)
            {
                // TODO TO Add A PRompt
                Console.WriteLine(e);
            }
            return Collection;
        }

        /// <summary>
        /// Property Change Interface Implementation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propertyname));
        }
        /// <summary>
        /// End
        /// </summary>

        private void GetRecent()
        {
            RecentFiles = RetriveCollection("temp.tmp");
            if (RecentFiles == null)
            {
                return;
            }
        }


        /// <summary>
        /// Commands for Page Controls
        /// </summary>

        public CommandBase GetOnlineWallpaper { get; set; }
        public CommandBase GetAbstractWallpaper { get; set; }
        public CommandBase GetLocalWallpaper { get; set; }
        public CommandBase SetWallpaper { get; set; }
        public KeyBinding UpdateWallpaper { get; set; }
        /// <summary>
        /// End
        /// </summary>
    }
}
