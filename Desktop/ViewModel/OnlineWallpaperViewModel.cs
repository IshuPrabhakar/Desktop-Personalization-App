using Desktop.Commands;
using Desktop.FileDropHelper;
using Desktop.Helper;
using Desktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PexelsDotNetSDK;
using RestSharp;
using RestSharp.Authenticators;
using System.Windows;

namespace Desktop.ViewModel
{
    public class OnlineWallpaperViewModel
    {
        public SettingHelper Helper { get; set; }
        public ObservableCollection<Online> Wallpapers { get; set; }
        public string API_URL { get; set; }
        public string Api_Key { get; set; }
        public string PexelsSearchURL { get; set; }
        public string SearchParam { get; set; }
        public string NextPageUrl { get; set; }

        public OnlineWallpaperViewModel()
        {
            Helper = new SettingHelper();

            Api_Key = Properties.Settings.Default.ApiKey;
            API_URL = Properties.Settings.Default.ApiUrl;
            PexelsSearchURL = Properties.Settings.Default.SearchUrl;
            Wallpapers = new ObservableCollection<Online>();

            _ = GetFiles();

            // COMMAND INVOCATION
            ScrollChanged = new CommandBase(LoadMore);
            PreviewWallpaper = new CommandBase(Preview);
            GetResult = new CommandBase(GetSearchResults);
            // END
        }

        private async void LoadMore(object obj)
        {
            if (Helper.IsConnectedToTheInternet() == false)
            {
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.ShowNotification("Can't Search", "Ensure that you have proper internet connection");
            }
            else
            {
                await Task.Run(() => GetFiles(NextPageUrl));
            }
        }

        private async void GetSearchResults(object obj)
        {
            if (Helper.IsConnectedToTheInternet() == false)
            {
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.ShowNotification("Can't Search", "Ensure that you have connected to the internet");
            }
            else
            {
                string searchParam = obj as string;
                Wallpapers.Clear();
                _ = GetFiles(PexelsSearchURL + "query=" + searchParam + "&per_page=16");
            }
        }

        private void Preview(object obj)
        {
            Button Selected = obj as Button;
            Online SelectedWallpaper = Selected.DataContext as Online;
            BitmapImage bitmap;

            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(SelectedWallpaper.medium, UriKind.Absolute);
            bitmap.DecodePixelWidth = 760;
            bitmap.DecodePixelHeight = 460;
            bitmap.EndInit();

            Properties.Settings.Default.CurrentWallpaperPath = bitmap.ToString();
            Properties.Settings.Default.Save();

        }

        private async Task GetFiles(string searchParam)
        {
            try
            {
                RestClient client = new RestClient(searchParam);
                RestRequest request = new RestRequest();
                request.AddHeader("Authorization", Api_Key);
                var response = await client.GetAsync(request);

                PexelsCurated Searchphotos = JsonConvert.DeserializeObject<PexelsCurated>(response.Content);

                BitmapImage bitmap;
                foreach (PexelsPhoto item in Searchphotos.Photos)
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(item.src.Tiny, UriKind.Absolute);
                    bitmap.DecodePixelWidth = 285;
                    bitmap.DecodePixelHeight = 152;
                    bitmap.EndInit();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Wallpapers.Add(new Online(item.src.Original.Substring(item.src.Original.IndexOf("pexels-"), item.src.Original.Length - item.src.Original.IndexOf("pexels-")), item.src.Original, item.src.Large, item.src.Large2x, item.src.Medium, item.src.Small, item.src.Portrait, item.src.Landscape, item.src.Tiny, bitmap));

                    });
                }
                NextPageUrl = Searchphotos.Next_page;

            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private async Task GetFiles()
        {
            try
            {
                RestClient client = new RestClient(API_URL);
                RestRequest request = new RestRequest();
                request.AddHeader("Authorization", Api_Key.ToString());
                var response = await client.GetAsync(request);

                PexelsCurated Curatedphotos = JsonConvert.DeserializeObject<PexelsCurated>(response.Content);

                BitmapImage bitmap;
                foreach (PexelsPhoto item in Curatedphotos.Photos)
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(item.src.Tiny, UriKind.Absolute);
                    bitmap.DecodePixelWidth = 285;
                    bitmap.DecodePixelHeight = 152;
                    bitmap.EndInit();

                    Wallpapers.Add(new Online(item.src.Original.Substring(item.src.Original.IndexOf("pexels-"), item.src.Original.Length - item.src.Original.IndexOf("pexels-")), item.src.Original, item.src.Large, item.src.Large2x, item.src.Medium, item.src.Small, item.src.Portrait, item.src.Landscape, item.src.Tiny, bitmap));

                }
                NextPageUrl = Curatedphotos.Next_page;
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Commands for Page Controls
        /// </summary>
        public CommandBase ScrollChanged { get; set; }
        public CommandBase PreviewWallpaper { get; set; }
        public CommandBase GetResult { get; set; }
        /// <summary>
        /// End
        /// </summary>

    }
}
