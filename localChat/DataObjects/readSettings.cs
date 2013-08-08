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
        public int upTime;
        public bool recieveToastNotificaiton;

        public readSettings()
        {
            /* Test.. */
            radiusMetersIndx = 5;
            keepTime = 21;
            upTime = 15;
            recieveToastNotificaiton = true;
        }
    }
}
