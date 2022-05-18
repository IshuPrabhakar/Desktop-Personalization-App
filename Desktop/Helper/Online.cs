using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Desktop.Helper
{
    public class Online
    {

        public Online(string Filename, string original, string large, string large2x, string medium, string small, string portrait, string landscape, string tiny, BitmapImage filePath)
        {
            this.Filename = Filename;
            this.original = original;
            this.large = large;
            this.large2x = large2x;
            this.medium = medium;
            this.small = small;
            this.portrait = portrait;
            this.landscape = landscape;
            this.tiny = tiny;
            FilePath = filePath;
        }
        public string Filename { get; set; }

        public string original { get; set; }

        public string large { get; set; }

        public string large2x { get; set; }

        public string medium { get; set; }

        public string small { get; set; }

        public string portrait { get; set; }

        public string landscape { get; set; }

        public string tiny { get; set; }

        public BitmapImage FilePath { get; set; }
    }
}
