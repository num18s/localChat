using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class latLong
    {
        private static int REMOVE_DECIMAL = 100000;
        private static int LAT_ADD = 90, LON_ADD = 180;
        private static float LAT_SEGMENT = 1, LON_SEGMENT = 1;

        private float lat, lon;
        private float sysLat, sysLon;
        private string gridP = "p.", grid1 = "1.", grid2 = "2.", grid3 = "3.", grid4 = "4."
            , grid5 = "5.", grid6 = "6.", grid7 = "7.", grid8 = "8.";

        latLong(float lat, float lon)
        {
            //Set variables
            this.lat = lat;
            this.lon = lon;
            sysLat = this.lat + LAT_ADD + REMOVE_DECIMAL;
            sysLon = this.lon + LON_ADD + REMOVE_DECIMAL;

            //Calculate local sysLat/Lon without minutes (degrees only)
            //Use to calculate grids
            float sysLatDeg = (float)( (int)( this.lat + LAT_ADD ) );
            float sysLonDeg = (float)( (int)( this.lon + LON_ADD ) );

            gridP += ( (int)( sysLatDeg / LAT_SEGMENT ) ).ToString();
            gridP += ".";
            gridP += ( (int)(sysLonDeg / LON_SEGMENT) ).ToString();

            gridP += ((int)( ( sysLatDeg - ( LAT_SEGMENT / 2 ) ) / LAT_SEGMENT)).ToString();
            gridP += ".";
            gridP += ((int)(sysLonDeg / LON_SEGMENT)).ToString();
        }
    }
}
