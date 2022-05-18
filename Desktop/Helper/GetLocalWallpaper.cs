using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Desktop.Model;

namespace Desktop.Helper
{
    public class GetLocalWallpaper
    {
        public ObservableCollection<Wallpaper> Wallpaper { get; set; }
        public ObservableCollection<string> Folders { get; set; }
        public string FileInDirectory { get; set; }
        public string folder { get; set; }

        public ObservableCollection<Wallpaper> GetWallPaper(string FolderName)
        {
            Wallpaper = new ObservableCollection<Wallpaper>();

            foreach (string Files in Directory.GetFiles(FolderName))
            {
                if (Path.GetFileName(Files).Contains("System$"))
                {
                    Wallpaper.Add(new Wallpaper()
                    {
                        FileName = Path.GetFileNameWithoutExtension(Files.Substring(Files.IndexOf("@") + 1)),
                        FilePath = Files
                    });
                }
            }

            return Wallpaper;
        }

        public string GetFolder()
        {
            return (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media\Thumbnails").ToString();
        }

        public ObservableCollection<Wallpaper> GetCollection()
        {
            String FolderName = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media\Thumbnails").ToString();

            ObservableCollection<Wallpaper> Collection = new ObservableCollection<Wallpaper>();

            foreach (string Files in Directory.GetFiles(FolderName))
            {
                if (Path.GetFileName(Files).Contains("Local."))
                {
                    Collection.Add(new Wallpaper()
                    {
                        FileName = Path.GetFileNameWithoutExtension(Files[(Files.IndexOf("@") + 1)..]),
                        FilePath = Files
                    });
                }
            }
            return Collection;
        }
    }
}

