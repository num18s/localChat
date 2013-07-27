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
    public partial class ReadDetailsPage : PhoneApplicationPage
    {
        MessageItem curReadMsg;
        // Constructor
        public ReadDetailsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string msgId = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out msgId))
                {
                    int index = int.Parse(msgId);   // cast for DB retrival..

                    readData readMsg = new readData();  //TODO: this one currently will create a new object and rebuild 50 msg... not working...
                    msg curMsg = readMsg.getMsg(index);
                    curReadMsg = new MessageItem()
                    {
                        //dbMsgID = curMsg.msgID.ToString(),
                        Date = curMsg.createDate.Date.ToString("MM/dd/yyyy"),
                        Time = curMsg.createDate.Date.ToString("HH:mm:ss tt"),
                        Title = curMsg.title,
                        Author = curMsg.userName,
                        Msg = curMsg.msgBody
                    };

                    DataContext = curReadMsg;

                    //DataContext = App.ReadMsgList.Items[index];
                }
            }
        }

        private void readDetailPrev_click(object sender, EventArgs e)
        {
            /* get the current msg index */
            int i = App.ReadMsgList.getCurMsgIndex(curReadMsg.dbMsgID);

            /* get the previous msgId in a circular buffer */
            if (i == 0) i = App.ReadMsgList.CurrentItemCount() - 1;
            else i--;

            string msgId = App.ReadMsgList.Items[i].dbMsgID;
            NavigationService.Navigate(new Uri("/ReadDetailsPage.xaml?selectedItem=" + msgId, UriKind.Relative));
        }

        private void readDetailNext_click(object sender, EventArgs e)
        {
            /* get the current msg index */
            int i = App.ReadMsgList.getCurMsgIndex(curReadMsg.dbMsgID);

            /* get the next msgId in a circular buffer */
            if (i == (App.ReadMsgList.CurrentItemCount() - 1)) i = 0;
            else i++;

            string msgId = App.ReadMsgList.Items[i].dbMsgID;
            NavigationService.Navigate(new Uri("/ReadDetailsPage.xaml?selectedItem=" + msgId, UriKind.Relative));
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