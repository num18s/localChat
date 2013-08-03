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

using localChat.DataObjects;

using System.ComponentModel;

namespace localChat
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private BackgroundWorker bwUsername;

        // Constructor
        public LoginPage()
        {
            InitializeComponent();

            this.bwUsername = new System.ComponentModel.BackgroundWorker();
            this.bwUsername.DoWork += new System.ComponentModel.DoWorkEventHandler(this.changeUsernameDSDoWork);
            this.bwUsername.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.changeUsernameDSComplete);
        }

        private void btnUsername_Click(object sender, RoutedEventArgs e)
        {
            btnContiune.IsEnabled = false;
            btnLink.IsEnabled = false;
            btnAnonymous.IsEnabled = false;

            string username = txtUsername.Text;
            bwUsername.RunWorkerAsync(username);
        }

        public void changeUsernameDSDoWork(object sender, DoWorkEventArgs e)
        {
            dataSource ds = App.Current.getDataSource();
            string username = e.Argument.ToString();

            usernameChangeReturn passOut = ds.changeUsername(username);

            e.Result = passOut;
        }

        public void changeUsernameDSComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("An error occurred, please try again");
                App.SaveDebugEntry("LoginPage.changeUsernameDSComplete: Canceld");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation. 
                MessageBox.Show("An error occurred, please try again");
                App.SaveDebugEntry("LoginPage.changeUsernameDSComplete: error occured");
            }
            else
            {
                usernameChangeReturn errorMsg = (usernameChangeReturn)e.Result;
                if (errorMsg.error == 0)
                {
                    MessageBox.Show("Username successfully set!");
                    NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
                }
                else if (errorMsg.error == 1)
                {
                    MessageBox.Show("That username is already taken, please choose a different name");
                }
                else if (errorMsg.error == 2)
                {
                    MessageBox.Show("Username was already set for this user.");
                }
                   
                btnContiune.IsEnabled = true;
                btnLink.IsEnabled = true;
                btnAnonymous.IsEnabled = true;
            }
        }

        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
        }

        private void btnNoUsername_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
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