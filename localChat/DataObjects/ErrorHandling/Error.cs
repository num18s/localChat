using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localChat
{
    class Error
    {
        private ErrorType errorType = null;
        private string objectName, methodName, error;
        private DateTime timestamp;

        public ErrorType getErrorType(){ return errorType; }
        public string getObjectName(){ return objectName; }
        public string getMethodName(){ return methodName; }
        public string getError(){ return error; }
        public DateTime getTimestamp(){ return timestamp; }

        public Error(string _objectName, string _methodName, string _error, DateTime _timestamp)
        {
            objectName = _objectName;
            methodName = _methodName;
            error = _error;
            timestamp = _timestamp;
        }

        public Error(string delimited)
        {
            bool good = false;
            int pipe = 0;
            int nextPipe = delimited.IndexOf("|");

            string _objectName, _methodName, _error;
            _objectName = _methodName = _error = "";

            DateTime _timestamp = new DateTime(0);

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

                        pipe = nextPipe + 1;
                        nextPipe = delimited.IndexOf("|", pipe);

                        if( nextPipe > 0 ){
                            string strTicks = delimited.Substring(pipe, nextPipe - pipe);
                            int ticks = 0;
                            try
                            {
                                ticks = int.Parse(strTicks);
                            }
                            catch (FormatException e) { }

                            if( ticks > 0 ){
                                _timestamp = new DateTime( ticks );
                                good = true;
                            }
                        }
                        
                    }
                }
            }
            if (good){
                objectName = _objectName;
                methodName = _methodName;
                error = _error;
                timestamp = _timestamp;
            }
        }

        public bool addType( List<ErrorType> errorTypes ){
            if (errorType != null) return true;

            foreach (ErrorType aErrorType in errorTypes)
            {
                if (aErrorType.Equals(this))
                {
                    errorType = aErrorType;
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return objectName + "|" + methodName + "|" + error;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Error e = obj as Error;
            if ((System.Object)e == null) return false;
            
            return Equals( e );
        }

        public bool Equals(Error toCompare)
        {
            if ( objectName == toCompare.objectName
                && methodName == toCompare.methodName
                && error == toCompare.error
            ) return true;

            return false;
        }
    }
}
