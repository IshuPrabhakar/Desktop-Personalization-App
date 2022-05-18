using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop.Model
{
    public class Wallpaper
    {
        public Wallpaper() { }

        public Wallpaper(string Filename, string Filepath)
        {
            this.FileName = Filename;
            this.FilePath = Filepath;
        }

        public string FileName { get; set; }

        public string FilePath { get; set; }
    }


}
