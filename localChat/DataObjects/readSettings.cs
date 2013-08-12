using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace localChat
{
    public class readSettings
    {
        public int radiusMetersIndx;
        public int keepTime;
        public int updateInterval;
        public bool recieveToastNotificaiton;

        private float lat;
        private float lon;

        public float latStart;
        public float latEnd;
        public float lonStart;
        public float lonEnd;

        private const int version = 1;

        public readSettings()
        {
            radiusMetersIndx = 5;
            keepTime = 21;
            updateInterval = 15;
            recieveToastNotificaiton = true;
            latStart = latEnd = lonStart = lonEnd = 0;
            lat = lon = 0;
        }

        public int getVersion()
        {
            return version;
        }

        public void getCurrentLatLonRage()
        {
            /* Get Current location */
            GeoCoordinateWatcher getPosition = new GeoCoordinateWatcher();

            int geoGetTry = 3; //times...
            bool gotGeoRespond = false;

            while (geoGetTry-- > 0 && gotGeoRespond == false)
            {
                gotGeoRespond = getPosition.TryStart(false, TimeSpan.FromMilliseconds(1000));
            }

            /*
             * Only update current location if is good location
             * else just use what we have before
             */
            if (gotGeoRespond == true)
            {
                lat = (float)getPosition.Position.Location.Latitude;
                lon = (float)getPosition.Position.Location.Longitude;
            }

            getPosition.Stop();
            getPosition.Dispose();

            latLon position = new latLon(lat, lon);
            double r = (double)App.distancesMeter[App.ReadSettings.radiusMetersIndx] / (double)App.R;

            double latRad = mathPlus.DegreeToRadian((double)position.getLat());
            latStart = (float)mathPlus.RadianToDegree(latRad - r);
            latEnd = (float)mathPlus.RadianToDegree(latRad + r);

            double lonRad = mathPlus.DegreeToRadian((double)position.getLon());
            double tLon = Math.Asin(Math.Sin(r) / Math.Cos(latRad));
            lonStart = (float)mathPlus.RadianToDegree(lonRad - tLon);
            lonEnd = (float)mathPlus.RadianToDegree(lonRad + tLon);
        }
    }
}
