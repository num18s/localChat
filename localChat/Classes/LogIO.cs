using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.IsolatedStorage;
using System.IO;

namespace localChat
{
    static class LogIO
    {
        private static string LOG_PATH = "/errorLog.txt";
        private static string ERROR_PATH = "/errorMeta.txt";
        private static int LOG_SIZE = 100;

        static public void logError( string msg ){
            using(
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication() 
            ){
            
                if( !store.FileExists( LOG_PATH ) ){
                    IsolatedStorageFileStream debutTxt = store.CreateFile(LOG_PATH);
                    debutTxt.Close();
                }

                using( 
                    StreamWriter sw = new StreamWriter( store.OpenFile(LOG_PATH,FileMode.Append,FileAccess.Write ) )
                ){
                    sw.WriteLine(msg);
                }

            }
        }

        static public List<string> getLog()
        {
            using (
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()
            )
            {
                using (
                    StreamReader sr = new StreamReader(store.OpenFile(LOG_PATH, FileMode.Open, FileAccess.Read))
                )
                {
                    List<string> output = new List<string>(LOG_SIZE);
                    int i = 0;
                    while( !sr.EndOfStream )
                        output.Add( sr.ReadLine() );
                    
                    return output;
                }
            }
        }

        static public void clearLog()
        {
           using(
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()
            ){
               if( store.FileExists( LOG_PATH ) )
                   store.DeleteFile( LOG_PATH );
           }    
        }

        static public List<ErrorType> getErrorTypes()
        {
            using (
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()
            )
            {
                using (
                    StreamReader sr = new StreamReader(store.OpenFile(ERROR_PATH, FileMode.Open, FileAccess.Read))
                )
                {
                    List<ErrorType> output = new List<ErrorType>(LOG_SIZE);

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        bool good = false;
                        int pipe = 0;
                        int nextPipe = line.IndexOf("|");

                        int id = 0;
                        string objectName, methodName, msg;
                        objectName = methodName = msg = "";

                        if (nextPipe > 0)
                        {
                            try
                            {
                                id = int.Parse(line.Substring(0, line.IndexOf("|")));
                            }
                            catch (FormatException e) { }

                            pipe = nextPipe + 1;
                            nextPipe = line.IndexOf("|", pipe);

                            if (nextPipe > 0)
                            {
                                objectName = line.Substring(pipe, nextPipe - pipe);

                                pipe = nextPipe + 1;
                                nextPipe = line.IndexOf("|", pipe);

                                if (nextPipe > 0)
                                {
                                    methodName = line.Substring(pipe, nextPipe - pipe);
                                    msg = line.Substring(nextPipe + 1);
                                    good = true;
                                }
                            }
                        }

                        if( good )
                            output.Add(new ErrorType(id, objectName, methodName, msg));
                    }
                    return output;
                }
            }
        }

        static public void setErrorTypes(List<ErrorType> errorTypes)
        {
            using (
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()
            )
            {
                if( store.FileExists( ERROR_PATH ) )
                   store.DeleteFile( ERROR_PATH );

                if (!store.FileExists(ERROR_PATH))
                {
                    IsolatedStorageFileStream debutTxt = store.CreateFile(ERROR_PATH);
                    debutTxt.Close();
                }

                using (
                    StreamWriter sw = new StreamWriter(store.OpenFile(ERROR_PATH, FileMode.Append, FileAccess.Write))
                )
                {

                    foreach (ErrorType errorType in errorTypes)
                        sw.WriteLine(errorType.getID() + "|" + errorType.getObject() + "|" + errorType.getMethod() + "|" + errorType.getMsg());
                }

            }
        }
    }
}
