using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class dataSource
    {
        private string key;
        private int userID;

        private static int R = 6371 * 1000;//6371 circuferance of earth in kilometers, converted to meeters

        public dataSource( string key ){
            this.key = key;
        }

        bool write(float lat, float lon, int radiusMeters, string msg)
        {
            latLon position = new latLon(lat, lon);
            double r = (double)radiusMeters / (double)R;
            int gridI = -1;

            double latRad = mathPlus.DegreeToRadian( (double)position.getLat() );
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

            for (int i = 0; i < latLon.getGridCount(); i++)
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

            //Do DB work

            return true;
        }
    }
}
