using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Helper
{
    public class SettingHelper
    {
        public string CreateConfigutionFilePath(string FileName)
        {
            String configFilePath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Configs").ToString();
            if (!Directory.Exists(configFilePath))
            {
                _ = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName);
                _ = Directory.CreateDirectory(configFilePath);
                if (!File.Exists(configFilePath + @"\" + FileName))
                {
                    _ = File.Create(configFilePath + @"\" + FileName);
                }
            }
            return configFilePath + @"\" + FileName;
        }

        public string CreateMediaFilePath(string FileName)
        {
            String configFilePath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media").ToString();
            if (!Directory.Exists(configFilePath))
            {
                _ = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName);
                _ = Directory.CreateDirectory(configFilePath);
                if (!File.Exists(configFilePath + @"\" + FileName))
                {
                    _ = File.Create(configFilePath + @"\" + FileName);
                }
            }
            return configFilePath + @"\" + FileName;
        }

        public string CreateThumbFilePath(string FileName)
        {
            String FilePath = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media\Thumbnails").ToString();
            if (!Directory.Exists(FilePath))
            {
                _ = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName);
                _ = Directory.CreateDirectory(FilePath);
                if (!File.Exists(FilePath + @"\" + FileName))
                {
                    _ = File.Create(FilePath + @"\" + FileName);
                }
            }
            return FilePath + @"\" + FileName;
        }

        public void Save(Object DATA, String FileName, Type DataType)
        {
            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(CreateConfigutionFilePath(FileName)))
                {
                    JsonWriter jsonWriter = new JsonTextWriter(sw);
                    jsonSerializer.Serialize(jsonWriter, DATA);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public object Retrive(String FileName, Type DataType)
        {
            //TODO To Add Encryption
            // "gConfig.secure"
            JObject obj = new JObject();
            obj = null;
            try
            {
                if (!File.Exists(CreateConfigutionFilePath(FileName)))
                {
                    _ = File.Create(CreateConfigutionFilePath(FileName));
                    return obj;
                }
                if (new FileInfo(CreateConfigutionFilePath(FileName)).Length != 0)
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    using StreamReader textReader = new StreamReader(CreateConfigutionFilePath(FileName));
                    JsonReader jsonReader = new JsonTextReader(textReader);
                    obj = jsonSerializer.Deserialize(jsonReader) as JObject;
                }

            }
            catch (Exception e)
            {
                // TODO TO Add A PRompt
                Console.WriteLine(e);
            }
            object DataObject = obj.ToObject(DataType);
            return DataObject;
        }

        //set infomation for image
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);


        public void SetWallpaper(string FilePath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, FilePath, SPIF_UPDATEINIFILE);
        }

        public void MakeThumbNails(string TAG, string ImagePath)
        {
            string Thumbnail = "Thumb." + TAG + new DirectoryInfo(Path.GetDirectoryName(ImagePath)).Name + "@" + Path.GetFileName(ImagePath);
            using (MagickImage magic = new(ImagePath))
            {
                magic.Thumbnail(new MagickGeometry(350, 200));
                magic.Write(CreateThumbFilePath(Thumbnail));
            };
        }

        public string MakePreview(string ImagePath, string TAG = "Preview")
        {
            string Thumbnail = "Thumb." + TAG + new DirectoryInfo(Path.GetDirectoryName(ImagePath)).Name + "@" + Path.GetFileName(ImagePath);
            using (MagickImage magic = new(ImagePath))
            {
                magic.Thumbnail(new MagickGeometry(800, 400));
                magic.Write(CreateThumbFilePath(Thumbnail));
            };
            return CreateThumbFilePath(Thumbnail);
        }

        public void MakeSystemWallpapersThumbnail()
        {
            foreach (string directory in Directory.GetDirectories(Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Web\Wallpaper")))
            {
                foreach (string Files in Directory.GetFiles(directory))
                {
                    if (Files.EndsWith(".ini"))
                    {
                        continue;
                    }
                    MakeThumbNails("System$", Files);
                }
            }

        }

        public string ThumbToFileMap(string FileName)
        {
            string folder = FileName.Substring(FileName.IndexOf("$") + 1, FileName.IndexOf("@")- FileName.IndexOf("$") - 1);

            String CollectionFolder = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media\").ToString();
            string Mapping;

            if (FileName.Contains(".System$"))
            {
                Mapping = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Web\Wallpaper") + @"\" + folder + @"\" + FileName.Substring(FileName.IndexOf("@") + 1);
            }
            else
            {
                Mapping = CollectionFolder + FileName.Substring(FileName.IndexOf("@") + 1);
            }
            return Mapping;

        }

        public void MakeLocalWallpapersThumbnail()
        {
            String CollectionFolder = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Process.GetCurrentProcess().ProcessName + @"\" + Process.GetCurrentProcess().ProcessName + @"\Media").ToString();
            foreach (string Files in Directory.GetFiles(CollectionFolder))
            {
                MakeThumbNails("Local.", Files);
            }

        }

        public bool IsConnectedToTheInternet()
        {
            Ping ping = new Ping();
            try
            {
               PingReply reply = ping.Send("192.168.1.3", 1000);
                if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TimedOut)
                    return true;
            }
            catch { }
            return false;
        }
    }
}
