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

        public Settings()
        {
            InitializeComponent();
            pageLoaded = true;
        }

        private void slider_receive_radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                int radiusMeters = (int)slider_receive_radius.Value;
                curMeterValue.Text = radiusMeters.ToString();
            }
        }

        private void slider_keep_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                int keepTime = (int)slider_keep_time.Value;
                curTimeValue.Text = keepTime.ToString();
            }
        }

        private void slider_update_time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                int upTime = (int)slider_update_time.Value;
                curUpTimeValue.Text = upTime.ToString();
            }
        }
    }

}