using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace localChat
{
    public partial class Settings : PhoneApplicationPage
    {
        bool pageLoaded = false;

        private static string[] distances = { "1 Block",
            "1/8 Mile",
            "1/4 Mile",
            "1/2 Mile",
            "3/4 Mile",
            "1 Mile",
            "2 Mile",
            "3 Mile",
            "4 Mile",
            "5 Mile",
            "6 Mile",
            "7 Mile",
            "8 Mile",
            "9 Mile",
            "10 Mile"
                                            };

        int radiusMetersIndx = 5;
        int keepTime = 2;
        int updateInterval = 3;
        bool recieveToastNotificaiton = false;

        public Settings()
        {
            InitializeComponent();

            slider_receive_radius.Value = App.ReadSettings.radiusMetersIndx;
            curMeterValue.Text = distances[(int)slider_receive_radius.Value];

            slider_keep_time.Value = App.ReadSettings.keepTime;
            curTimeValue.Text = slider_keep_time.Value.ToString();

            slider_update_time.Value = App.ReadSettings.updateInterval;
            curUpdateIntervalValue.Text = slider_update_time.Value.ToString();

            recieve_toast_notificaiton_cb.IsChecked = App.ReadSettings.recieveToastNotificaiton;

            pageLoaded = true;
        }

        private void slider_receive_radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                radiusMetersIndx = (int)slider_receive_radius.Value;
                curMeterValue.Text = distances[radiusMetersIndx];
            }
        }

        private void slider_keep_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                keepTime = (int)slider_keep_time.Value;
                curTimeValue.Text = keepTime.ToString();
            }
        }

        private void slider_update_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                updateInterval = (int)slider_update_time.Value;
                curUpdateIntervalValue.Text = updateInterval.ToString();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult saveMsg = MessageBox.Show("Please click ok if you really want to save the read settings?", "Save Settings?", MessageBoxButton.OKCancel);
            if (saveMsg == MessageBoxResult.OK)
            {
                App.ReadSettings.keepTime = keepTime;
                App.ReadSettings.radiusMetersIndx = radiusMetersIndx;
                App.ReadSettings.updateInterval = updateInterval;
                App.ReadSettings.recieveToastNotificaiton = recieveToastNotificaiton;

                FileStorageOperations.saveReadSettings();

                NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
        }

        private void recieve_toast_notificaiton_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (pageLoaded)
            {
                recieveToastNotificaiton = recieve_toast_notificaiton_cb.IsChecked.Value;
            }
        }
        
    }

}