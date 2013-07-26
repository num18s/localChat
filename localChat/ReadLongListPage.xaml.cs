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
                /* progresss bar... */
                pi = new Microsoft.Phone.Shell.ProgressIndicator();
                pi.IsIndeterminate = true;
                pi.Text = "Receiving message, please wait...";
                pi.IsVisible = true;
                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, true);
                Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, pi);

                //App.ReadMsgList.LoadData();
                if (readMsg == null)
                {
                    readMsg = new readData();
                }

                int numMsg = readMsg.getLength();

                for (int i = 0; i < numMsg; i++)
                {
                    msg curMsg = readMsg.getMsg(i);
                    MessageItem incomingMsg = new MessageItem()
                    {
                        ID = curMsg.msgID.ToString(),
                        Date = curMsg.createDate.Date.ToString("MM/dd/yyyy"),
                        Time = curMsg.createDate.Date.ToString("HH:mm:ss tt"),
                        Title = curMsg.title,
                        Author = curMsg.userName,
                        Msg = curMsg.msgBody
                    };

                    App.ReadMsgList.Items.Add(incomingMsg);
                }
                                
                pi.IsVisible = false;
                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);
            }
        }

        // Handle selection changed on LongListSelector
        private void IncomingMessageLLS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (IncomingMessageLLS.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/ReadDetailsPage.xaml?selectedItem=" + (IncomingMessageLLS.SelectedItem as MessageItem).ID, UriKind.Relative));

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