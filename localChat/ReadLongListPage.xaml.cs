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

namespace localChat
{
    public partial class ReadLongListPage : PhoneApplicationPage
    {
        private Microsoft.Phone.Shell.ProgressIndicator pi;

        private static readData readMsg = null;

        // Constructor
        public ReadLongListPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ReadMsgList;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ReadMsgList.IsDataLoaded)
            {
                App.ReadMsgList.LoadData();

                if (readMsg == null)
                {
                    readMsg = new readData();
                }
            }

            /* Remove all all back entry from the stack.. */
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            /* progresss bar... */
            pi = new Microsoft.Phone.Shell.ProgressIndicator();
            pi.IsIndeterminate = true;
            pi.Text = "Receiving message, please wait...";
            pi.IsVisible = true;
            Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, true);
            Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, pi);
            /*
              * Push all the msg from DB to MessageGroup so we can keep
              * a list of latest 50 message from DB
              */
            int numMsg = readMsg.getLength();
            for (int i = 0; i < numMsg; i++)
            {
                msg curDBMsg = readMsg.getMsg(i);
                MessageItem incomingMsg = new MessageItem()
                {
                    dbMsgID = curDBMsg.msgID.ToString(),
                    Date = curDBMsg.createDate.Date.ToString("MM/dd/yyyy"),
                    Time = curDBMsg.createDate.Date.ToString("HH:mm:ss tt"),
                    Title = curDBMsg.title,
                    //Author = curDBMsg.userName,
                    //Msg = curDBMsg.msgBody
                };

                /* Add to local list for retrive later.. */
                App.ReadMsgList.Items.Add(incomingMsg);
            }

            pi.IsVisible = false;
            Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);
            DataContext = App.ReadMsgList;

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
            Thread work = new Thread(new ThreadStart(Refresh_Click_Work));
            work.Start();
        }

        private void Refresh_Click_Work()
        {
            dataSource ds = new dataSource("12345678910");
            ds.read();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Settings button works!");
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            //Do work for your application here.
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This app is brought you by the hard working people like Matthew Dryden and Bohao She.  Please enjoy our hard work and let us know what you think!  Thank you.");
            //Do work for your application here.
        }


        //UNUSED...
        private void ReadLongListPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                /* Handel all the file saving... */

                throw new Exception("ExitAppException");    // only way I can find to exit the app...
            }
            else
                e.Cancel = true; // Tell the system that we got it..
        }

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