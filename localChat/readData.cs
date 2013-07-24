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
