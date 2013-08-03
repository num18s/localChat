using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public class UserReturn
    {
        public statusMsg status;
        public string imsi;
        public long imsiID, userID;
        public string username, email;
        public int userStatus;
        public DateTime createDate, modifyDate;

        public UserReturn() { }
    }
}
