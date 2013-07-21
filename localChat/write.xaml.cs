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
    public partial class WritePage : PhoneApplicationPage
    {
        bool gotId = false;
        String myId = String.Empty;

        public WritePage()
        {
            InitializeComponent();

            if (!gotId)
            {
                byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                myId = Convert.ToBase64String(myDeviceID);
                gotId = true;
            }
        }

        private void Post_Button_Click(object sender, RoutedEventArgs e)
        {
            String title = this.Message_Title_Box.Text;
            String message = this.Message_Post_Box.Text;
            int id = App.ReadMsgList.CurrentItemCount();

            MessageItem outgoingMsg = new MessageItem()
                    {
                        ID = id.ToString(),
                        Date = DateTime.Now.ToString("MM/dd/yyyy"),
                        Time = DateTime.Now.ToString("HH:mm:ss tt"),
                        Title = title,
                        Author = myId,
                        Msg = message
                    };

            App.ReadMsgList.Items.Add(outgoingMsg);

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/ReadLongListPage.xaml", UriKind.Relative));
        }
    }
}