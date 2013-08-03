using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.ComponentModel;

namespace localChat
{
    public partial class SplashScreen : PhoneApplicationPage
    {
        private Microsoft.Phone.Shell.ProgressIndicator pi;

        private BackgroundWorker bw;

        public SplashScreen()
        {
            InitializeComponent();

            //Loading Bar
            pi = new Microsoft.Phone.Shell.ProgressIndicator();
            pi.IsIndeterminate = true;
            pi.Text = "Loading User Data...";
            pi.IsVisible = true;
            Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, true);
            Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, pi);

            this.bw = new System.ComponentModel.BackgroundWorker();
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.createDSDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.createDSComplete);

            this.bw.RunWorkerAsync();
        }

        public void createDSDoWork(object sender, DoWorkEventArgs e)
        {
            App.Current.setDataSource(new dataSource());
        }

        public void createDSComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            pi.IsVisible = false;
            Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);

            if (App.Current.getDataSource() == null)
            {
                MessageBox.Show("Failed to contact the remote server, please try again latter");
            }
            else if (App.Current.getDataSource().getUser().getStatus() != 0)
            {
                NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
        }
    }
}