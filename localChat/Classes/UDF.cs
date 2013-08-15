using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    static public class UDF
    {
        static public bool bool2bool(bool? toConvert)
        {
            if (toConvert.Value == null || !toConvert.Value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
