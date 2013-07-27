using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Device.Location;
using Newtonsoft.Json;

namespace localChat
{
    class dataSource
    {
        private string key;
        private int userID;

        private static int R = 6371 * 1000;//6371 circuferance of earth in kilometers, converted to meeters

        public dataSource(string key)
        {
            this.key = key;
        }

        public bool write(int radiusMeters, string title, string msg)
        {
            GeoCoordinateWatcher getPosition = new GeoCoordinateWatcher();
            getPosition.TryStart(false, TimeSpan.FromMilliseconds(1000));

            float lat = (float)getPosition.Position.Location.Latitude;
            float lon = (float)getPosition.Position.Location.Longitude;

            getPosition.Stop();
            getPosition.Dispose();

            latLon position = new latLon(lat, lon);
            double r = (double)radiusMeters / (double)R;
            int gridI = -1;

            double latRad = mathPlus.DegreeToRadian((double)position.getLat());
            float latStart = (float)mathPlus.RadianToDegree(latRad - r);
            float latEnd = (float)mathPlus.RadianToDegree(latRad + r);

            double lonRad = mathPlus.DegreeToRadian((double)position.getLon());
            double tLon = Math.Asin(Math.Sin(r) / Math.Cos(latRad));
            float lonStart = (float)mathPlus.RadianToDegree(lonRad - tLon);
            float lonEnd = (float)mathPlus.RadianToDegree(lonRad + tLon);

            latLon nw = new latLon(latStart, lonStart);
            latLon ne = new latLon(latStart, lonEnd);
            latLon se = new latLon(latEnd, lonEnd);
            latLon sw = new latLon(latEnd, lonStart);

            string[] nwGrid = nw.getGrids();
            string[] neGrid = ne.getGrids();
            string[] seGrid = se.getGrids();
            string[] swGrid = sw.getGrids();

            for (int i = 0; i < position.getGridCount(); i++)
            {
                if (nwGrid[i] == neGrid[i]
                && nwGrid[i] == seGrid[i]
                && nwGrid[i] == swGrid[i])
                {
                    gridI = i;
                    break;
                }
            }

            if (gridI < 0) return false;

            StringBuilder output = new StringBuilder("");

            DB db = new DB();
            db.setOutput(output);

            db.write(0
                , nwGrid[gridI]
                , position.getLat()
                , position.getLon()
                , position.getSysLat()
                , position.getSysLon()
                , nw.getSysLat()
                , sw.getSysLat()
                , nw.getSysLon()
                , ne.getSysLon()
                , radiusMeters
                , title
                , msg
            );

            statusMsg status = JsonConvert.DeserializeObject<statusMsg>(output.ToString());

            return true;
        }

        public readData read()
        {
            GeoCoordinateWatcher getPosition = new GeoCoordinateWatcher();
            getPosition.TryStart(false, TimeSpan.FromMilliseconds(1000));

            float lat = (float)getPosition.Position.Location.Latitude;
            float lon = (float)getPosition.Position.Location.Longitude;

            getPosition.Stop();
            getPosition.Dispose();

            latLon position = new latLon(lat, lon);

            StringBuilder htmlOutput = new StringBuilder("");

            DB db = new DB();
            db.setOutput(htmlOutput);

            db.read(position);

            readData output = JsonConvert.DeserializeObject<readData>(htmlOutput.ToString());

            return output;
        }
    }
}
