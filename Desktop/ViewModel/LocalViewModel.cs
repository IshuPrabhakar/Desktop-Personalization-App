using Desktop.Helper;
using Desktop.Model;
using System;
using Desktop.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Desktop.Frames;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Desktop.FileDropHelper;

namespace Desktop.ViewModel
{
    public class LocalViewModel : INotifyPropertyChanged, OnFileDrop
    {
        public ObservableCollection<Wallpaper> LocalWallpaper { get; set; }
        public ObservableCollection<Wallpaper> CollectionFiles { get; set; }
        public ObservableCollection<Wallpaper> Recent { get; set; }
        public Config config { get; set; }

        public SettingHelper Helper { get; set; }

        public LocalViewModel()
        {
            Helper = new SettingHelper();
            config = new Config();
            LocalWallpaper = new ObservableCollection<Wallpaper>();
            CollectionFiles = new ObservableCollection<Wallpaper>();
            Recent = new ObservableCollection<Wallpaper>();

            // Load Collection
            GetCollections();
            GetWallPapers();

            // COMMAND INVOCATION
            PreviewWallpaper = new CommandBase(Preview);
            SetLocalWallpaper = new CommandBase(SetLocal);
            SetSystemWallpaper = new CommandBase(SetSystem);
            AddToCollection = new CommandBase(Add);
            // END
        }

        private void Add(object obj)
        {
            Wallpaper SelectedWallpaper = obj as Wallpaper;
            Helper.MakeThumbNails("Local.", SelectedWallpaper.FilePath);
            CollectionFiles.Clear();
            GetCollections();
        }

        private void SetSystem(object obj)
        {
            Wallpaper SelectedWallpaper = obj as Wallpaper;

            try
            {
                Helper.SetWallpaper(Helper.ThumbToFileMap(SelectedWallpaper.FilePath));

                Properties.Settings.Default.CurrentWallpaperPath = Helper.ThumbToFileMap(SelectedWallpaper.FilePath);
                Properties.Settings.Default.Save();


                Recent.Add(SelectedWallpaper);
                Helper.Save(Recent, "temp.tmp", typeof(Wallpaper));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void SetLocal(object obj)
        {
            Wallpaper SelectedWallpaper = obj as Wallpaper;
            try
            {
                Helper.SetWallpaper(Helper.ThumbToFileMap(SelectedWallpaper.FilePath));
                Properties.Settings.Default.CurrentWallpaperPath = Helper.ThumbToFileMap(SelectedWallpaper.FilePath);
                Properties.Settings.Default.Save();

                Recent.Add(SelectedWallpaper);
                Helper.Save(Recent, "temp.tmp", typeof(Wallpaper));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Preview(object obj)
        {
            Button Selected = obj as Button;
            Wallpaper SelectedWallpaper = Selected.DataContext as Wallpaper;
            Properties.Settings.Default.CurrentWallpaperPath = Helper.MakePreview(Helper.ThumbToFileMap(SelectedWallpaper.FilePath));
            Properties.Settings.Default.Save();


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

        private void GetCollections()
        {
            GetLocalWallpaper Coll = new();
            CollectionFiles = Coll.GetCollection();
        }

        private void GetWallPapers()
        {
            GetLocalWallpaper localWallpaper = new GetLocalWallpaper();
            LocalWallpaper = localWallpaper.GetWallPaper(localWallpaper.GetFolder());
        }

        public void OnfileDropAction(string[] FilePaths)
        {
            String MediaFilePath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media\").ToString();

            try
            {
                File.Copy(FilePaths[0], MediaFilePath + Path.GetFileName(FilePaths[0]), true);
                Helper.MakeThumbNails("Local.", MediaFilePath + Path.GetFileName(FilePaths[0]));
                GetCollections();
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Commands for Page Controls
        /// </summary>
        public CommandBase PreviewWallpaper { get; set; }
        public CommandBase SetLocalWallpaper { get; set; }
        public CommandBase AddToCollection { get; set; }
        public CommandBase SetSystemWallpaper { get; set; }
        /// <summary>
        /// End
        /// </summary>

    }
}
