using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    public class readData
    {
        public statusMsg status;
        public msg[] msgs = new msg[50];
        public int length;

        public readData()
        {
        }

        public void addData(msg input)
        {
            msgs[length] = input;
            length++;
        }

        public msg getMsg( int i )
        {
            if( i > length ) return null;
            return msgs[i];
        }

        public int getLength(){
            return length;
        }
    }
}
