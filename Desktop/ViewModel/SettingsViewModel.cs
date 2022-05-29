using Desktop.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private string apiUrl;
        private string apiKey;
        private string searchUrl;
        private bool loopOn;
        private int loopFrequency;

        public string SearchUrl { get => searchUrl; set { searchUrl = value; OnPropertyChanged("searchUrl"); } }
        public string ApiUrl { get => apiUrl; set { apiUrl = value; OnPropertyChanged("apiUrl"); } }
        public string ApiKey { get => apiKey; set { apiKey = value; OnPropertyChanged("apiKey"); } }
        public bool LoopOn { get => loopOn; set { loopOn = value; OnPropertyChanged("loopOn"); } }
        public int LoopFrequency { get => loopFrequency; set { loopFrequency = value; OnPropertyChanged("loopFrequency"); } }

        public SettingsViewModel()
        {
            ApiKey = Properties.Settings.Default.ApiKey;
            SearchUrl = Properties.Settings.Default.SearchUrl;
            ApiUrl = Properties.Settings.Default.ApiUrl;
            LoopOn = Properties.Settings.Default.LoopOn;
            int time = (int)Properties.Settings.Default.LoopFrequency;
            switch (time)
            {
                case 10 * 1000 * 60:
                    LoopFrequency = 1;
                    break;
                case 30 * 1000 * 60:
                    LoopFrequency = 2;
                    break;
                case 60 * 24 * 60 * 1000:
                    LoopFrequency = 4;
                    break;
                case 60 * 1000 * 60:
                    LoopFrequency = 3;
                    break;
                case 5 * 1000 * 60:
                    LoopFrequency = 0;
                    break;
            }

            // COMMAND INVOCATION
            SaveSettings = new CommandBase(Save);
            LinkToDeveloper = new CommandBase(Link);
            // END
        }

        private void Link(object obj)
        {
            System.Diagnostics.Process.Start("https://www.github.com/IshuPrabhakar");
        }

        private void Save(object obj)
        {

            Properties.Settings.Default.ApiUrl = ApiUrl;
            Properties.Settings.Default.ApiKey = ApiKey;
            Properties.Settings.Default.SearchUrl = SearchUrl;
            Properties.Settings.Default.LoopOn = LoopOn;
            int time = 0;
            switch (LoopFrequency)
            {
                case 1:
                    time = 10 * 1000 * 60;
                    break;
                case 2:
                    time = 30 * 1000 * 60;
                    break;
                case 0:
                    time = 5 * 1000 * 60;
                    break;
                case 3:
                    time = 60 * 1000 * 60;
                    break;
                case 4:
                    time = 60 * 24 * 60 * 1000;
                    break;

                default:
                    time = 5;
                    break;
            }
            Properties.Settings.Default.LoopFrequency = time;
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

        /// <summary>
        /// Commands for Page Controls
        /// </summary>
        public CommandBase SaveSettings { get; set; }
        public CommandBase LinkToDeveloper { get; set; }
        /// <summary>
        /// End
        /// </summary>
    }
}
