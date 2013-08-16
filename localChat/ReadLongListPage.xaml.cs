
#define DEBUG_AGENT

using System;
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
using Microsoft.Phone.Scheduler;

namespace localChat
{

    public partial class ReadLongListPage : PhoneApplicationPage
    {
        private Microsoft.Phone.Shell.ProgressIndicator pi;

        private static readData readMsg = null;
        private BackgroundWorker bw;
        private bool doingRefresh = false;

        private int newMsgThisTime = 0;

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

            //if (App.ReadSettings.recieveToastNotificaiton)
            //    StartBackgroundAgent();
            //else
            //    StopBackgroundAgent();

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

            if (!App.ReadMsgList.IsDataLoaded)
            {
                /* Get Current location */
                App.ReadSettings.getCurrentLatLonRage();
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

                    newMsgThisTime = 0;

                    //App.ReadMsgList = new MessageGroup();

                    int numMsg = readMsg.getLength();
                    for (int i = 0; i < numMsg; i++)
                    {
                        msg curDBMsg = readMsg.getMsg(i);

                        TimeSpan keepDays = new System.TimeSpan(App.ReadSettings.keepTime, 0, 0, 0);

                        /* Only add message if is newer than what we want to see */
                        if (DateTime.Now.Subtract(keepDays).CompareTo(curDBMsg.createDate) > 0)
                            continue;   // msg is too old..

                        MessageItem incomingMsg = new MessageItem()
                        {
                            dbMsgID = curDBMsg.msgID.ToString(),
                            Date = curDBMsg.createDate.Date.ToString("MM/dd/yyyy"),
                            Time = curDBMsg.createDate.TimeOfDay.ToString(),
                            Title = curDBMsg.title,
                            Lat = curDBMsg.lat,
                            Lon = curDBMsg.lon,
                            CreateDate = curDBMsg.createDate
                            //Author = curDBMsg.userName,
                            //Msg = curDBMsg.msgBody
                        };

                        if(App.ReadMsgList.msgInsert(incomingMsg))
                            newMsgThisTime++;
                    }

                    updateLiveTile(newMsgThisTime);
                }
                pi.IsVisible = false;
                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);
                doingRefresh = false;
                DataContext = App.ReadMsgList;
            }
        }

        private void updateLiveTile(int msgCount)
        {
            /* Update title contents */
            var mainTile = ShellTile.ActiveTiles.FirstOrDefault();

            if (null != mainTile)
            {
                FlipTileData tileData = new FlipTileData()
                {
                    Count = msgCount
                };

                switch (msgCount)
                {
                    case 0:
                        tileData.BackContent = string.Empty;
                        break;
                    case 1:
                        tileData.BackContent = "You have " + msgCount + " post you have not read!";
                        break;
                    default:
                        tileData.BackContent = "You have " + msgCount + " posts you have not read!";
                        break;
                }

                mainTile.Update(tileData);
            }
        }

        PeriodicTask periodicTask = null;
        string periodicTaskName = "localChat";

        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }

        private void StopBackgroundAgent()
        {
            RemoveAgent(periodicTaskName);
        }

        private void StartBackgroundAgent()
        {
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            // If the task already exists and the IsEnabled property is false, background
            // agents have been disabled by the user
            if (periodicTask != null && !periodicTask.IsEnabled)
            {
                MessageBox.Show("Background agents for this application have been disabled by the user.");
                return;
            }

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null && periodicTask.IsEnabled)
            {
                RemoveAgent(periodicTaskName);
            }

            periodicTask = new PeriodicTask(periodicTaskName);

            // The description is required for periodic agents. This is the string that the user
            // will see in the background services Settings page on the device.
            periodicTask.Description = "EarReach Background Agent";
            ScheduledActionService.Add(periodicTask);

            // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG_AGENT)
            ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(App.ReadSettings.updateInterval*60));
#else
            ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromMinutes(App.ReadSettings.updateInterval * 60));
#endif
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