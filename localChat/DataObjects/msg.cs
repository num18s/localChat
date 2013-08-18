using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public class msg
    {
        public long msgID, userID;
        public string userName, title, msgBody;
        public DateTime createDate;
        public float lat, lon;
        public bool showLocation;
    }
}
