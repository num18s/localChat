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

        int radiusMetersIndx = 5;
        int keepTime = 2;
        int updateInterval = 3;

        public Settings()
        {
            InitializeComponent();

            slider_receive_radius.Value = radiusMetersIndx = App.ReadSettings.radiusMetersIndx;
            curMeterValue.Text = App.distances[(int)slider_receive_radius.Value];

            slider_keep_time.Value = keepTime = App.ReadSettings.keepTime;
            curTimeValue.Text = slider_keep_time.Value.ToString();

            slider_update_time.Value = updateInterval = App.ReadSettings.updateInterval;
            curUpdateIntervalValue.Text = slider_update_time.Value.ToString();

            recieve_toast_notificaiton_cb.IsChecked = App.ReadSettings.recieveToastNotificaiton;

            pageLoaded = true;
        }

        private void slider_receive_radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                radiusMetersIndx = (int)slider_receive_radius.Value;
                curMeterValue.Text = App.distances[radiusMetersIndx];
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
                App.ReadSettings.recieveToastNotificaiton = recieve_toast_notificaiton_cb.IsChecked.Value;

                /* Update message list to take out message that is too old.. */
                /* Remove message that is too old.. */
                TimeSpan keepDays = new System.TimeSpan(App.ReadSettings.keepTime, 0, 0, 0);

                for (int i = 0; i < App.ReadMsgList.CurrentItemCount(); )
                {
                    /* Only add message if is newer than what we want to see */
                    if (DateTime.Now.Subtract(keepDays).CompareTo(App.ReadMsgList.Items[i].CreateDate) > 0)
                        // msg is too old..
                        App.ReadMsgList.Items.Remove(App.ReadMsgList.Items[i]);
                    else
                        i++;
                }

                FileStorageOperations.saveReadSettings();

                NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
        }
    }

}