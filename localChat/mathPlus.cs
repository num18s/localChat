using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public static class  mathPlus
    {
        //Code taken from: http://www.vcskicks.com/csharp_net_angles.php
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        //Code taken from: http://www.vcskicks.com/csharp_net_angles.php
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
