using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class ErrorNoID
    {
        private string objectName, methodName, error;

        public string getObjectName(){ return objectName; }
        public string getMethodName(){ return methodName; }
        public string getError(){ return error; }

        public ErrorNoID(string _objectName, string _methodName, string _error)
        {
            objectName = _objectName;
            methodName = _methodName;
            error = _error;
        }

        public ErrorNoID(string delimited)
        {
            bool good = false;
            int pipe = 0;
            int nextPipe = delimited.IndexOf("|");

            string _objectName, _methodName, _error;
            _objectName = _methodName = _error = "";

            if (nextPipe > 0)
            {
                _objectName = delimited.Substring(0, delimited.IndexOf("|") );

                pipe = nextPipe + 1;
                nextPipe = delimited.IndexOf("|", pipe);
                
                if (nextPipe > 0)
                {
                    _methodName = delimited.Substring(0, delimited.IndexOf("|") );

                    pipe = nextPipe + 1;
                    nextPipe = delimited.IndexOf("|", pipe);

                    if (nextPipe > 0)
                    {
                        _error = delimited.Substring(pipe, nextPipe - pipe);
                        good = true;
                    }
                }
            }
            if (good){
                objectName = _objectName;
                methodName = _methodName;
                error = _error;
            }
        }

        public override string ToString()
        {
            return objectName + "|" + methodName + "|" + error;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            ErrorNoID e = obj as ErrorNoID;
            if ((System.Object)e == null) return false;
            
            return Equals( e );
        }

        public bool Equals(ErrorNoID toCompare)
        {
            if ( objectName == toCompare.objectName
                && methodName == toCompare.methodName
                && error == toCompare.error
            ) return true;

            return false;
        }
    }
}
