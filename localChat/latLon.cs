using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class latLon
    {
        private static int REMOVE_DECIMAL = 100000;
        private static int LAT_ADD = 90, LON_ADD = 180;
        private static float LAT_SEGMENT = 1, LON_SEGMENT = 1;
        private static int GRID_COUNT = 9;

        private float lat, lon;
        private int sysLat, sysLon;
        private string[] grid = new string[GRID_COUNT];

        public latLon(float lat, float lon)
        {
            //Set variables
            this.lat = lat;
            this.lon = lon;
            sysLat = (int)( ( this.lat + LAT_ADD ) * REMOVE_DECIMAL );
            sysLon = (int)( ( this.lon + LON_ADD ) * REMOVE_DECIMAL );

            //Calculate local sysLat/Lon without minutes (degrees only)
            //Use to calculate grids
            float sysLatDeg = (float)( (int)( this.lat + LAT_ADD ) );
            float sysLonDeg = (float)( (int)( this.lon + LON_ADD ) );
            float temp = 0;

            if (this.lat > 84) 
            {
                grid[0] = grid[1] = grid[2] = grid[3] = grid[4] = grid[5] = grid[6] = grid[7] = grid[8] = "N";
            }
            else if (this.lat < -80)
            {
                grid[0] = grid[1] = grid[2] = grid[3] = grid[4] = grid[5] = grid[6] = grid[7] = grid[8] = "S";
            }
            else
            {
                grid[0] = "p.";//Prime
                grid[1] = "1.";
                grid[2] = "2.";
                grid[3] = "3.";
                grid[4] = "4.";
                grid[5] = "5.";
                grid[6] = "6.";
                grid[7] = "7.";
                grid[8] = "8.";


                grid[0] += ((int)(sysLatDeg / LAT_SEGMENT)).ToString();
                grid[0] += ".";
                grid[0] += ((int)(sysLonDeg / LON_SEGMENT)).ToString();

                //North, West
                grid[1] += ( (int)( ( sysLatDeg + ( LAT_SEGMENT / 2 ) ) / LAT_SEGMENT) ).ToString();
                grid[1] += ".";
                temp = (sysLonDeg - ( LON_SEGMENT / 2 ));
                if( temp < 0 ) temp = ( LON_ADD * 2 ) - temp;
                grid[1] += ( (int)(temp / LON_SEGMENT ) ).ToString();

                //North, East
                grid[2] += ((int)((sysLatDeg + (LAT_SEGMENT / 2)) / LAT_SEGMENT)).ToString();
                grid[2] += ".";
                grid[2] += ((int)((sysLonDeg + (LON_SEGMENT / 2)) / LON_SEGMENT)).ToString();

                //South, East
                grid[3] += ((int)((sysLatDeg - (LAT_SEGMENT / 2)) / LAT_SEGMENT)).ToString();
                grid[3] += ".";
                grid[3] += ((int)((sysLonDeg + (LON_SEGMENT / 2)) / LON_SEGMENT)).ToString();

                //South, West
                grid[4] += ((int)((sysLatDeg - (LAT_SEGMENT / 2)) / LAT_SEGMENT)).ToString();
                grid[4] += ".";
                temp = (sysLonDeg - (LON_SEGMENT / 2));
                if (temp < 0) temp = (LON_ADD * 2) - temp;
                grid[4] += ((int)(temp / LON_SEGMENT)).ToString();

                //Same, West
                grid[5] += ((int)(sysLatDeg / LAT_SEGMENT)).ToString();
                grid[5] += ".";
                temp = (sysLonDeg - (LON_SEGMENT / 2));
                if (temp < 0) temp = (LON_ADD * 2) - temp;
                grid[5] += ((int)(temp / LON_SEGMENT)).ToString();

                //North, Same
                grid[6] += ((int)((sysLatDeg + (LAT_SEGMENT / 2)) / LAT_SEGMENT)).ToString();
                grid[6] += ".";
                grid[6] += ((int)(sysLonDeg / LON_SEGMENT)).ToString();

                //Same, East
                grid[7] += ((int)(sysLatDeg / LAT_SEGMENT)).ToString();
                grid[7] += ".";
                grid[7] += ((int)((sysLonDeg + (LON_SEGMENT / 2)) / LON_SEGMENT)).ToString();

                //South, Same
                grid[8] += ((int)((sysLatDeg - (LAT_SEGMENT / 2)) / LAT_SEGMENT)).ToString();
                grid[8] += ".";
                grid[8] += ((int)(sysLonDeg / LON_SEGMENT)).ToString();
            }
        }

        public float getLat(){
            return lat;
        }

        public float getLon()
        {
            return lon;
        }

        public int getSysLat()
        {
            return sysLat;
        }

        public int getSysLon()
        {
            return sysLon;
        }

        public string[] getGrids()
        {
            string[] output = new string[GRID_COUNT];
            for( int i = 0; i < GRID_COUNT; i++ )
                output[i] = grid[i];

            return output;
        }

        public static int getGridCount()
        {
            return GRID_COUNT;
        }
    }
}
