using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class readData
    {
        private msg[] msgs = new msg[50];
        private int length = 0;

        public readData()
        {

            //public long msgID, userID;
            //public string userName, title, msgBody;
            //public DateTime createDate;
            //public float lat, lon;

            for (length = 0; length < 50; )
            {
                msg temp = new msg()
                {
                    msgID = length,
                    title = "msg" + length,
                    createDate = DateTime.Now,
                    msgBody = "testing msg " + length,
                    userName = "user " + length
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
