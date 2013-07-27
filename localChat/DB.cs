using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Threading;

namespace localChat
{
    class DB
    {
        private string logInUrl = "https://securec55.ezhostingserver.com/feildofbattlecards-com/localChat/test.html";
        private string readUrl = "http://feildofbattlecards-com.securec55.ezhostingserver.com/localChat/read_msg.cfm";
        private string readDetailsUrl = "http://feildofbattlecards-com.securec55.ezhostingserver.com/localChat/read_msg_details.cfm";
        private string writeUrl = "http://feildofbattlecards-com.securec55.ezhostingserver.com/localChat/create_msg.cfm";
        private string logUrl = "https://securec55.ezhostingserver.com/feildofbattlecards-com/localChat/test.html";

        private StringBuilder output = null;

        WebClient client = new WebClient();
        private EventWaitHandle asyncWait = new ManualResetEvent(false);

        public DB()
        {
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(getOutput);
        }

        public void setOutput(StringBuilder output)
        {
            output.Clear();
            this.output = output;
        }

        public void read( latLon position )
        {
            string[] grids = position.getGrids();

            string parameter = "?cf_gridP=" + grids[0];
            for (int i = 1; i < position.getGridCount(); i++)
                parameter += "&cf_grid" + i + "=" + grids[i];
            
            parameter += "&cf_sys_lat=" + position.getSysLat();
            parameter += "&cf_sys_lon=" + position.getSysLon();

            Uri readUri = new Uri(readUrl + parameter, UriKind.Absolute);

            client.OpenReadAsync(readUri);
            asyncWait.WaitOne(2000);
        }

        public void readDetails(long msg_id)
        {
            string parameter = "?cf_msg_id=" + msg_id;

            Uri readUri = new Uri(readDetailsUrl + parameter, UriKind.Absolute);

            client.OpenReadAsync(readUri);
            asyncWait.WaitOne(2000);
        }

        public void write(long user_id
            , string grid
            , float lat
            , float lon
            , int sysLat
            , int sysLon
            , int sysLatStart
            , int sysLatEnd
            , int sysLonStart
            , int sysLonEnd
            , int radiusMeters
            , string title
            , string msg
        )
        {
            string parameter = "?cf_user_id=" + user_id.ToString();
            parameter += "&cf_grid=" + grid.ToString();
            parameter += "&cf_lat=" + lat.ToString();
            parameter += "&cf_lon=" + lon.ToString();
            parameter += "&cf_sys_lat=" + sysLat.ToString();
            parameter += "&cf_sys_lon=" + sysLon.ToString();
            parameter += "&cf_sys_lat_start=" + sysLatStart.ToString();
            parameter += "&cf_sys_lat_end=" + sysLatEnd.ToString();
            parameter += "&cf_sys_lon_start=" + sysLonStart.ToString();
            parameter += "&cf_sys_lon_end=" + sysLonEnd.ToString();
            parameter += "&cf_radius_meters=" + radiusMeters.ToString();
            parameter += "&cf_title=" + title;
            parameter += "&cf_msg=" + msg;

            Uri writeUri = new Uri(writeUrl + parameter, UriKind.Absolute);

            client.OpenReadAsync(writeUri);
            asyncWait.WaitOne(2000);
        }

        private void getOutput(Object sender, OpenReadCompletedEventArgs e)
        {
            Stream reply = null;
            StreamReader reader = null;

            try
            {
                reply = (Stream)e.Result;
                reader = new StreamReader(reply);
                output.Append(reader.ReadToEnd());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (reply != null)
                {
                    reply.Close();
                }
            }
            output = null;
            asyncWait.Set();
        }
    }
}
