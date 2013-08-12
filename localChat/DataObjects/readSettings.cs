using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public class readSettings
    {
        public int radiusMetersIndx;
        public int keepTime;
        public int updateInterval;
        public bool recieveToastNotificaiton;

        public float latStart;
        public float latEnd;
        public float lonStart;
        public float lonEnd;

        public readSettings()
        {
            /* Test.. */
            radiusMetersIndx = 5;
            keepTime = 21;
            updateInterval = 15;
            recieveToastNotificaiton = true;
        }
    }
}
