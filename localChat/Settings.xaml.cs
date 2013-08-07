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

        int radiusMeters = 500;
        int keepTime = 2;
        int upTime = 3;
        bool recieveToastNotificaiton = false;

        public Settings()
        {
            InitializeComponent();

            slider_receive_radius.Value = App.ReadSettings.radiusMeters;
            curMeterValue.Text = slider_receive_radius.Value.ToString();

            slider_keep_time.Value = App.ReadSettings.keepTime;
            curTimeValue.Text = slider_keep_time.Value.ToString();

            slider_update_time.Value = App.ReadSettings.upTime;
            curUpTimeValue.Text = slider_update_time.Value.ToString();

            recieve_toast_notificaiton_cb.IsChecked = App.ReadSettings.recieveToastNotificaiton;

            pageLoaded = true;
        }

        private void slider_receive_radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                radiusMeters = (int)slider_receive_radius.Value;
                curMeterValue.Text = radiusMeters.ToString();
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
                upTime = (int)slider_update_time.Value;
                curUpTimeValue.Text = upTime.ToString();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult saveMsg = MessageBox.Show("Please click ok if you really want to save the read settings?", "Save Settings?", MessageBoxButton.OKCancel);
            if (saveMsg == MessageBoxResult.OK)
            {
                App.ReadSettings.keepTime = keepTime;
                App.ReadSettings.radiusMeters = radiusMeters;
                App.ReadSettings.upTime = upTime;
                App.ReadSettings.recieveToastNotificaiton = recieveToastNotificaiton;

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