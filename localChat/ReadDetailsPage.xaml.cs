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
    public partial class ReadDetailsPage : PhoneApplicationPage
    {
        private int msgID;
        private MessageItem curReadMsg;
        private BackgroundWorker bw;
        
        // Constructor
        public ReadDetailsPage()
        {
            InitializeComponent();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.getMsgDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.getMsgComplete);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string strMsgId = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out strMsgId))
            msgID = Convert.ToInt32(strMsgId);

            if (DataContext == null)
            {
                getMsg();
                
            }
        }

        private void getMsg()
        {
            this.bw.RunWorkerAsync(msgID);
        }

        private void getMsgDoWork(object sender, DoWorkEventArgs e)
        {
            int msgID = (int)e.Argument;
            dataSource ds = new dataSource("12345678910");
            readData passBack = ds.readDetails(msgID);
            e.Result = passBack;
        }

        private void getMsgComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation. 
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                readData msgOutput = (readData)e.Result;
                //Error Handling, need to provide feedback to the user
                if (msgOutput != null)
                {
                    msg curMsg = msgOutput.getMsg(0);

                    curReadMsg = new MessageItem()
                    {
                        dbMsgID = curMsg.msgID.ToString(),
                        Date = curMsg.createDate.Date.ToString("MM/dd/yyyy"),
                        Time = curMsg.createDate.Date.ToString("HH:mm:ss tt"),
                        Title = curMsg.title,
                        Author = curMsg.userName,
                        Msg = curMsg.msgBody
                    };

                    DataContext = curReadMsg;
                }
            }
        }

        private void readDetailPrev_click(object sender, EventArgs e)
        {
            if (curReadMsg != null)
            {
                /* get the current msg index */
                int i = App.ReadMsgList.getCurMsgIndex(curReadMsg.dbMsgID);

                /* get the previous msgId in a circular buffer */
                if (i == 0) i = App.ReadMsgList.CurrentItemCount() - 1;
                else i--;

                string msgId = App.ReadMsgList.Items[i].dbMsgID;
                NavigationService.Navigate(new Uri("/ReadDetailsPage.xaml?selectedItem=" + msgId, UriKind.Relative));
            }
        }

        private void readDetailNext_click(object sender, EventArgs e)
        {
            if (curReadMsg != null)
            {
                /* get the current msg index */
                int i = App.ReadMsgList.getCurMsgIndex(curReadMsg.dbMsgID);

                /* get the next msgId in a circular buffer */
                if (i == (App.ReadMsgList.CurrentItemCount() - 1)) i = 0;
                else i++;

                string msgId = App.ReadMsgList.Items[i].dbMsgID;
                NavigationService.Navigate(new Uri("/ReadDetailsPage.xaml?selectedItem=" + msgId, UriKind.Relative));
            }
        }

        private void ReadDetailsPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true; // Tell the system that we got it..
                        
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