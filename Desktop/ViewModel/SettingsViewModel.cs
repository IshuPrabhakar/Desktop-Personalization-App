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
        private string loopFrequency;

        public string SearchUrl { get => searchUrl; set { searchUrl = value; OnPropertyChanged("searchUrl"); } }
        public string ApiUrl { get => apiUrl; set { apiUrl = value; OnPropertyChanged("apiUrl"); } }
        public string ApiKey { get => apiKey; set { apiKey = value; OnPropertyChanged("apiKey"); } }
        public bool LoopOn { get => loopOn; set { loopOn = value; OnPropertyChanged("loopOn"); } }
        public string LoopFrequency { get => loopFrequency; set { loopFrequency = value; OnPropertyChanged("loopFrequency"); } }

        public SettingsViewModel()
        {
            ApiKey = Properties.Settings.Default.ApiKey;
            SearchUrl = Properties.Settings.Default.SearchUrl;
            ApiUrl = Properties.Settings.Default.ApiUrl;
            loopOn = Properties.Settings.Default.LoopOn;
            LoopFrequency = Properties.Settings.Default.LoopFrequency;

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
            Properties.Settings.Default.LoopFrequency = LoopFrequency;
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
