﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class ErrorType
    {
        private int id;
        private string objectName, methodName, msg;

        public ErrorType(int _id, string _object, string _method, string _msg )
        {
            id = _id;
            objectName = _object;
            methodName = _method;
            msg = _msg;
        }

        public int getID() { return id; }
        public string getObject() { return objectName; }
        public string getMethod() { return methodName; }
        public string getMsg() { return msg; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            ErrorType et = obj as ErrorType;
            if ((System.Object)et == null) return false;

            return Equals( et );
        }

        public bool Equals(ErrorType toCompare)
        {
            if (id == toCompare.id)
                return true;

            return false;
        }
    }
}
