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

        private const string removeFavUri = "/Assets/Appbar/unlike.png";
        private const string FavUri = "/Assets/Appbar/like.png";
        
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

            if (!App.ReadMsgList.IsDataLoaded)
            {
                /* We just got a request from a tile for just for reading that mssage.*/
                /* Not workinng yet.. */
                //App.Current.setDataSource(new dataSource());
                NavigationService.Navigate(new Uri("/SplashScreen.xaml", UriKind.Relative));
            }

            if (DataContext == null)
            {
                getMsg();
            }
            
            SetPinBar();

            base.OnNavigatedTo(e);
        }

        private void getMsg()
        {
            this.bw.RunWorkerAsync(msgID);
        }

        private void getMsgDoWork(object sender, DoWorkEventArgs e)
        {
            int msgID = (int)e.Argument;
            dataSource ds = App.Current.getDataSource();
            readData passBack = ds.readDetails(msgID);
            e.Result = passBack;
        }

        private void getMsgComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("An error occurred, please try again");
                App.SaveDebugEntry("ReadDetailsPage.getMsgComplete: Canceld");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation. 
                MessageBox.Show("An error occurred, please try again");
                App.SaveDebugEntry("ReadDetailsPage.getMsgComplete: error occured");
            }
            else
            {
                readData msgOutput = (readData)e.Result;
                //Error Handling, need to provide feedback to the user
                if (msgOutput == null)
                {
                    //msg is null, reason unknown
                    MessageBox.Show("An error occurred, please try again");
                    App.SaveDebugEntry("ReadDetailsPage.getMsgComplete: null readMsg");
                }
                else
                {

                    msg curMsg = msgOutput.getMsg(0);

                    curReadMsg = new MessageItem()
                    {
                        dbMsgID = curMsg.msgID.ToString(),
                        Date = curMsg.createDate.Date.ToString("MM/dd/yyyy"),
                        Time = curMsg.createDate.TimeOfDay.ToString(),
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

        private void btnPinToStart_Click(object sender, EventArgs e)
        {
            var uri = NavigationService.Source.ToString();
            if (Features.Tile.TileExists(uri))
            {
                Features.Tile.DeleteTile(uri);
            }
            else
            {
                Features.Tile.SetTile(curReadMsg, uri);
            }

            SetPinBar();
        }

        public ApplicationBarIconButton pinBtn
        {
            get
            {
                var appBar = (ApplicationBar)ApplicationBar;
                var count = appBar.Buttons.Count;
                for (var i = 0; i < count; i++)
                {
                    ApplicationBarIconButton btn = appBar.Buttons[i] as ApplicationBarIconButton;
                    if (btn.IconUri.OriginalString.Contains("like"))
                        return btn;
                }
                return null;
            }
        }

        void SetPinBar()
        {
            var uri = NavigationService.Source.ToString();
            if (Features.Tile.TileExists(uri))
            {
                pinBtn.IconUri = new Uri(removeFavUri, UriKind.Relative);
                pinBtn.Text = "Unpin";
            }
            else
            {
                pinBtn.IconUri = new Uri(FavUri, UriKind.Relative);
                pinBtn.Text = "Pin";
            }
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