using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Collections.ObjectModel;

namespace localChat
{
    public class messageData
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
    }

    public partial class ReadPanorama : PhoneApplicationPage
    {
        public ObservableCollection<messageData> readList;

        public ReadPanorama()
        {
            InitializeComponent();

            Loaded += readPage_Loaded;
        }

        void readPage_Loaded(object sender, RoutedEventArgs e)
        {
           //LoadReadData();
           //BindData();

           // App.MessageList.MessageLoadLocalData();
            App.MessageList.IncomingMessageLoaded += RecieveMessageLoaded;
        }

        void RecieveMessageLoaded(object sender, EventArgs e)
        {
            CurrentLB.ItemsSource = App.MessageList.MessageItemList;
            AlertLB.ItemsSource = App.MessageList.MessageItemList;
        }
        
        private void LoadReadData()
        {
            readList = new ObservableCollection<messageData>
                        {
                            new messageData
                                {
                                    UniqueId = "xyz",
                                    Title = "Test1",
                                    Date = "2/5/2013",
                                    Time = "5:30:54",
                                    Author = "ja ja bin",
                                    Message = "testing....."
                                }, 
                            new messageData
                                {
                                    UniqueId = "cadf",
                                    Title = "Test2",
                                    Date = "12/5/2013",
                                    Time = "3:50:54",
                                    Author = "yo yo ma",
                                    Message = "testing next....."
                                }, 
                        };
        }

        private void BindData()
        {           
            //CurrentLB.ItemsSource = readList;
            //AlertLB.ItemsSource = readList;
        }

        private void CurrentLB_SelectionChanged(object sender, EventArgs e)
        {
            MessageBox.Show("CurrentLB Selected");
            //Do work for your application here.
        }


        //private void CurrentLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (CurrentLB.SelectedItem != null)
        //    {
        //        NavigationService.Navigate(new Uri("/ReadDetail.xaml?ID=" +
        //            (CurrentLB.SelectedItem as messageData).UniqueId, UriKind.Relative));
        //    }
        //}

        private void Write_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Write button works!");
            //Do work for your application here.
            NavigationService.Navigate(new Uri("/write.xaml", UriKind.Relative));
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Refresh button works!");
            //Do work for your application here.
        }
        private void Settings_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Settings button works!");
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            //Do work for your application here.
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About works!");
            //Do work for your application here.
        }

		//private void AlertLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    MessageBox.Show("AlerLB Selection works!");
        //    // TODO: Add event handler implementation here.
        //}
    }
}