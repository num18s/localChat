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

using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation; //Provides the Geocoordinate class.
using System.Windows.Shapes;
using System.Windows.Media;

namespace localChat
{
    public partial class ReadDetailsPage : PhoneApplicationPage
    {
        private int msgID;
        private MessageItem curReadMsg;
        private BackgroundWorker bw;
        private BackgroundWorker bwDS;

        private const string removeFavUri = "/Assets/Appbar/unlike.png";
        private const string FavUri = "/Assets/Appbar/like.png";

        private Map MyMap = null;
        private GeoCoordinate postLoc = null;
        private Button showMap = null;
        private bool showMapToggle = true;
        
        // Constructor
        public ReadDetailsPage()
        {
            InitializeComponent();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.getMsgDoWork);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.getMsgComplete);

            this.bwDS = new System.ComponentModel.BackgroundWorker();
            this.bwDS.DoWork += new System.ComponentModel.DoWorkEventHandler(this.createDSDoWork);
            this.bwDS.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.createDSComplete);

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void btnShowMap_Click(object sender, RoutedEventArgs e)
        {
            if (showMapToggle)
            {
                showMap.Content = "Hide Map";
                addMap();
            }
            else
            {
                showMap.Content = "Show Map";
                ContentPanel.Children.Remove(MyMap);
            }
            showMapToggle = !showMapToggle;
        }

        public void addMap()
        {
            if (postLoc == null) return;

            MyMap = new Map();
            MyMap.Width = 440;
            MyMap.Height = 440;
            MyMap.Margin = new Thickness(0, -90, 0, 0);
            MyMap.Center = postLoc;
            MyMap.ZoomLevel = 13;
            ContentPanel.Children.Add(MyMap);

            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay postOverlay = new MapOverlay();
            postOverlay.Content = myCircle;
            postOverlay.PositionOrigin = new Point(0.5, 0.5);
            postOverlay.GeoCoordinate = postLoc;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer locationLayer = new MapLayer();
            locationLayer.Add(postOverlay);

            // Add the MapLayer to the Map.
            MyMap.Layers.Add(locationLayer);
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OnNavigatedToWork();

            base.OnNavigatedTo(e);
        }

        private void OnNavigatedToWork()
        {
            string strMsgId = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out strMsgId))
                msgID = Convert.ToInt32(strMsgId);

            if (App.Current.getDataSource() == null || App.Current.getDataSource().getUser() == null)
            {
                this.bwDS.RunWorkerAsync();
                return;
            }

            if (!App.ReadMsgList.IsDataLoaded)
            {
                App.ReadMsgList.LoadData(); // Get all the saved messags...
            }

            if (DataContext == null)
            {
                getMsg();
            }

            SetPinBar();
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
                        Msg = curMsg.msgBody,
                        ShowLocation = curMsg.showLocation
                    };

                    if (curMsg.showLocation)
                    {
                        postLoc = new GeoCoordinate(curMsg.lat, curMsg.lon);
                        msgBubble.Height = 400;

                        showMap = new Button();
                        showMap.Name = "btnShowMap";
                        showMap.Height = 75;
                        showMap.Width = 200;
                        showMap.Margin = new Thickness(226, 450, 0, 0);
                        showMap.Content = "Show Map";
                        showMap.Click += btnShowMap_Click;

                        ContentPanel.Children.Add(showMap);
                    }

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

        public void createDSDoWork(object sender, DoWorkEventArgs e)
        {
            App.Current.setDataSource(new dataSource());
        }

        public void createDSComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (App.Current.getDataSource().getUser() == null)
            {
                MessageBox.Show("Failed to contact the remote server, please try again latter");
                return;
            }

            OnNavigatedToWork();
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