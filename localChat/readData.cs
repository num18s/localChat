using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class readData
    {
        struct msg
        {
            long msgID, userID;
            DateTime createDate;
            string title, userName;
            float lat, lon;
        }

        public msg[] msgs = new msg[50];
        public int length = 0;

        public readData() { }

        public void addData(msg input)
        {
            msg[length] = input;
            length++;
        }
    }
}
