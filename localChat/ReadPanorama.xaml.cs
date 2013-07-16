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
    public partial class ReadPanorama : PhoneApplicationPage
    {
        public ReadPanorama()
        {
            InitializeComponent();
        }

        private void CurrentLB_SelectionChanged(object sender, EventArgs e)
        {
            MessageBox.Show("CurrentLB Selected");
            //Do work for your application here.
        }

        private void ListBox_SelectionChanged(object sender, EventArgs e)
        {
            MessageBox.Show("CurrentLB Selected");
            //Do work for your application here.
        }       
        private void Write_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Write button works!");
            //Do work for your application here.
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Refresh button works!");
            //Do work for your application here.
        }
        private void Settings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Settings button works!");
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            //Do work for your application here.
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About works!");
            //Do work for your application here.
        }
		
        //private void CurrentLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    MessageBox.Show("CurrentLB Selection works!");
        //    // TODO: Add event handler implementation here.
        //}
        //private void AlertLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    MessageBox.Show("AlerLB Selection works!");
        //    // TODO: Add event handler implementation here.
        //}
    }
}