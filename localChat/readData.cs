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
        public msg[] msgs;
        public int length;

        public readData() { }

        public void addData( msg input ){
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
