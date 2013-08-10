﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using localChat.Resources;
using System.Threading;
using System.ComponentModel;

using System.Device.Location;

namespace localChat
{

    public partial class ReadLongListPage : PhoneApplicationPage
    {
        private Microsoft.Phone.Shell.ProgressIndicator pi;

        private static readData readMsg = null;
        private BackgroundWorker bw;
        private bool doingRefresh = false;

        private static int R = 6371 * 1000;//6371 circuferance of earth in kilometers, converted to meeters

        private static int[] distancesMeter = { 107,
            201,
            402,
            804,
            1207,
            1609,
            3218,
            4828,
            6437,
            8016,
            9656,
            11265,
            12874,
            14484,
            16093
                                              };

        // Constructor
        public ReadLongListPage()
        {
            InitializeComponent();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.refreshDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.refreshCompleted);
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /* Remove all all back entry from the stack.. */
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            refreshData();
        }

        private void refreshData()
        {
            doingRefresh = true;
            /* progresss bar... */
            pi = new Microsoft.Phone.Shell.ProgressIndicator();
            pi.IsIndeterminate = true;
            pi.Text = "Receiving message, please wait...";
            pi.IsVisible = true;
            Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, true);
            Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, pi);

            //if (!App.ReadMsgList.IsDataLoaded)
            {
                /* Get Current location */
                GeoCoordinateWatcher getPosition = new GeoCoordinateWatcher();

                int geoGetTry = 3; //times...
                bool gotGeoRespond = false;

                while (geoGetTry-- > 0 || gotGeoRespond == false)
                {
                    gotGeoRespond = getPosition.TryStart(false, TimeSpan.FromMilliseconds(1000));
                }

                float lat = (float)getPosition.Position.Location.Latitude;
                float lon = (float)getPosition.Position.Location.Longitude;

                getPosition.Stop();
                getPosition.Dispose();

                latLon position = new latLon(lat, lon);
                double r = (double)distancesMeter[App.ReadSettings.radiusMetersIndx] / (double)R;

                double latRad = mathPlus.DegreeToRadian((double)position.getLat());
                App.ReadSettings.latStart = (float)mathPlus.RadianToDegree(latRad - r);
                App.ReadSettings.latEnd = (float)mathPlus.RadianToDegree(latRad + r);

                double lonRad = mathPlus.DegreeToRadian((double)position.getLon());
                double tLon = Math.Asin(Math.Sin(r) / Math.Cos(latRad));
                App.ReadSettings.lonStart = (float)mathPlus.RadianToDegree(lonRad - tLon);
                App.ReadSettings.lonEnd = (float)mathPlus.RadianToDegree(lonRad + tLon);

                App.ReadMsgList.LoadData(); // Get all the saved messags...
            }
            this.bw.RunWorkerAsync();
        }

        private void refreshDoWork(object sender, DoWorkEventArgs e)
        {
            dataSource ds = App.Current.getDataSource().copy();
            readData passBack = ds.read();
            e.Result = passBack;
        }

        private void refreshCompleted(
            object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("An error occurred, please try again");

               App.SaveDebugEntry("ReadLongListPage.refreshComplete: Canceld");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation. 
                MessageBox.Show("An error occurred, please try again");

                App.SaveDebugEntry("ReadLongListPage.refreshComplete: error occured");
            }
            else
            {
                /*
                * Push all the msg from DB to MessageGroup so we can keep
                * a list of latest 50 message from DB
                */
                readMsg = (readData)e.Result;

                if (readMsg == null)
                {
                    MessageBox.Show("An error occurred, please try again");

                    App.SaveDebugEntry("ReadLongListPage.refreshComplete: null readMsg");
                }
                else
                {
                    //App.SaveDebugEntry("ReadLongListPage.refreshComplete: test...");

                    App.ReadMsgList = new MessageGroup();

                    int numMsg = readMsg.getLength();
                    for (int i = 0; i < numMsg; i++)
                    {
                        msg curDBMsg = readMsg.getMsg(i);

                        MessageItem incomingMsg = new MessageItem()
                        {
                            dbMsgID = curDBMsg.msgID.ToString(),
                            Date = curDBMsg.createDate.Date.ToString("MM/dd/yyyy"),
                            Time = curDBMsg.createDate.TimeOfDay.ToString(),
                            Title = curDBMsg.title,
                            Lat = curDBMsg.lat,
                            Lon = curDBMsg.lon
                            //Author = curDBMsg.userName,
                            //Msg = curDBMsg.msgBody
                        };

                        /* Only add message if is not already loaded in the message list.. */
                        //if (App.ReadMsgList.getCurMsgIndex(incomingMsg.dbMsgID) == -1)
                        if(App.ReadMsgList.isInRange(incomingMsg))
                        {
                            /* Add to local list for retrive later.. */
                            App.ReadMsgList.Items.Add(incomingMsg);
                        }
                    }
                }
                pi.IsVisible = false;
                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);
                doingRefresh = false;
                DataContext = App.ReadMsgList;
            }
        }

        // Handle selection changed on LongListSelector
        private void IncomingMessageLLS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (IncomingMessageLLS.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/ReadDetailsPage.xaml?selectedItem=" + (IncomingMessageLLS.SelectedItem as MessageItem).dbMsgID, UriKind.Relative));

            // Reset selected item to null (no selection)
            IncomingMessageLLS.SelectedItem = null;
        }

        private void Write_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Write button works!");
            //Do work for your application here.
            NavigationService.Navigate(new Uri("/Write.xaml", UriKind.Relative));
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            if(!doingRefresh)
                refreshData();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Settings button works!");
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            //Do work for your application here.
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This app is brought you by the hard working people like Matthew Dryden and Bohao She.  Please enjoy our hard work and let us know what you think!\nThank you.");
            //Do work for your application here.
        }


        //UNUSED...
        //private void ReadLongListPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (NavigationService.CanGoBack)
        //    {
        //        /* Handel all the file saving... */

        //        throw new Exception("ExitAppException");    // only way I can find to exit the app...
        //    }
        //    else
        //        e.Cancel = true; // Tell the system that we got it..
        //}

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}