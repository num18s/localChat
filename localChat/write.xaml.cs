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
    public partial class WritePage : PhoneApplicationPage
    {
        bool gotId = false;
        String myId = String.Empty;
        private Microsoft.Phone.Shell.ProgressIndicator pi;

        private static string[] distances = { "1 Block",
            "1/8 Mile",
            "1/4 Mile",
            "1/2 Mile",
            "3/4 Mile",
            "1 Mile",
            "2 Mile",
            "3 Mile",
            "4 Mile",
            "5 Mile",
            "6 Mile",
            "7 Mile",
            "8 Mile",
            "9 Mile",
            "10 Mile"
                                            };

        private static int[] distancesMeter = { 107,
            201,
            402,
            804,
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




        bool pageLoaded = false;

        public WritePage()
        {
            InitializeComponent();

            if (!gotId)
            {
                byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                myId = Convert.ToBase64String(myDeviceID);
                gotId = true;
            }
            pageLoaded = true;
        }

        private void PostButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult postMsg = MessageBox.Show("Please click ok if you really want to post this message?", "Posting Message?", MessageBoxButton.OKCancel);
            if (postMsg == MessageBoxResult.OK)
            {
                String title = this.Message_Title_Box.Text;
                String message = this.Message_Post_Box.Text;
                int id = App.ReadMsgList.CurrentItemCount();
                int distance = (int)Distance_Slider.Value;
                int radiusMeters = distancesMeter[distance];

                MessageItem outgoingMsg = new MessageItem()
                         {
                             dbMsgID = id.ToString(),
                             Date = DateTime.Now.ToString("MM/dd/yyyy"),
                             Time = DateTime.Now.ToString("HH:mm:ss tt"),
                             Title = title,
                             Author = myId,
                             Msg = message
                         };

                /* progresss bar... */
                pi = new Microsoft.Phone.Shell.ProgressIndicator();
                pi.IsIndeterminate = true;
                pi.Text = "Posting message, please wait...";
                pi.IsVisible = true;
                Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, true);
                Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, pi);

                ThreadStart starter = delegate { Post_Button_Click_Work(title, message, id, radiusMeters); };

                Thread work = new Thread(starter);
                work.Start();

                ///* wait for message back from post to the cloud.. */
                //dataSource ds = new dataSource(myId);
                //ds.write(radiusMeters, title, message);

                //App.ReadMsgList.Items.Add(outgoingMsg);

                // Navigate to the new page
                NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
            }
        }

        private void Post_Button_Click_Work(string title, string message, int id, int radiusMeters)
        {
            dataSource ds = App.Current.getDataSource();
            ds.write(radiusMeters, title, message);

            //NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
        }

        private void Distance_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pageLoaded)
            {
                int distance = (int)Distance_Slider.Value;
                curMeterValue.Text = distances[distance];
            }
        }
    }
}