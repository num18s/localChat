using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public class readData
    {
        static public statusMsg status;
        static public msg[] msgs;
        static public int length;

        public readData()
        {
            //public long msgID, userID;
            //public string userName, title, msgBody;
            //public DateTime createDate;
            //public float lat, lon;

            for (; length < 50; )
            {
                msg temp = new msg()
                {
                    msgID = (length+1),
                    title = "msg" + (length+1),
                    createDate = DateTime.Now,
                    msgBody = "testing msg " + (length+2),
                    userName = "user " + (length+3)
                };
                addData(temp);
            }
        }

        public void addData(msg input)
        {
            msgs[length] = input;
            length++;
        }

        public msg getMsg( int i ){
            return msgs[i];
        }

        public int getLength(){
            return length;
        }
    }
}
