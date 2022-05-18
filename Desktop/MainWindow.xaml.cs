using Desktop.Helper;
using Desktop.ViewModel;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Forms = System.Windows.Forms;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SettingHelper Helper { get; set; }
        public Forms.NotifyIcon notifyIcon;

        public MainWindow()
        {
            Helper = new SettingHelper();

            InitializeComponent();

            // System Tray Icon, Notifications etc
            notifyIcon = new Forms.NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("Assets/AppIcon.ico");
            notifyIcon.Visible = true;

            if (Helper.IsConnectedToTheInternet() == false)
            {
                MainWindow main = Application.Current.MainWindow as MainWindow;
                main.ShowNotification("No Internet", "Ensure that you have connected to the internet.");
            }

            notifyIcon.Text = Process.GetCurrentProcess().ProcessName;
            notifyIcon.Click += MaximiseWindow;
        }

        private void MaximiseWindow(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            notifyIcon.Dispose();
            base.OnClosed(e);
        }

        public void ShowNotification(string Title, string Message)
        {
            notifyIcon.ShowBalloonTip(4000, Title, Message, Forms.ToolTipIcon.Info);
        }

        public void Move()
        {
            this.DragMove();
        }
    }
}
