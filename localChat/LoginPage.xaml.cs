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

using System.ComponentModel;

namespace localChat
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private BackgroundWorker bw;

        // Constructor
        public LoginPage()
        {
            InitializeComponent();

            this.bw = new System.ComponentModel.BackgroundWorker();
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.createDSDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.createDSComplete);

            if( App.Current.getDataSource() == null )
                this.bw.RunWorkerAsync();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        public void createDSDoWork(object sender, DoWorkEventArgs e)
        {
            App.Current.setDataSource( new dataSource() );
        }

        public void createDSComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            //if( App.Current.getDataSource().getUser().getUsername() == 
            //NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
        }

        private void btnUsername_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
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