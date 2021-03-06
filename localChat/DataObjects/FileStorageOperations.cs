﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;

using Newtonsoft.Json;

namespace localChat
{
    class FileStorageOperations
    {
        private static string READ_SETTING = "readSetting.txt";
        private static string MSG_GROUP_LIST = "msgGroup.txt";

        public async static Task SaveToLocalFolderAsync(string logData, string fileName, bool append)
        {

            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isoStore.FileExists(fileName))
            {
                IsolatedStorageFileStream dataFile = isoStore.CreateFile(fileName);
            }

            StreamWriter writeStream = new StreamWriter(new IsolatedStorageFileStream(fileName, (append) ? FileMode.Append : FileMode.Open, isoStore));
            writeStream.WriteLine(logData);
            writeStream.Close();

            // // Get a reference to the Local Folder
            // Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // // Create the file in the local folder, or if it already exists, just open it
            // Windows.Storage.StorageFile storageFile =
            // await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            // Stream writeStream = await storageFile.OpenStreamForWriteAsync();
            // using (StreamWriter writer = new StreamWriter(writeStream))
            // {
            // await writer.WriteAsync(logData);
            // writer.Close();
            // }
        }

        public async static Task<string> LoadFromLocalFolderAsync(string fileName)
        {
            string theData = string.Empty;

            // There's no FileExists method in WinRT, so have to try to get a reference to it
            // and catch the exception instead
            StorageFile storageFile = null;
            bool fileExists = false;
            try
            {
                // See if file exists
                storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appdata:///local/" + fileName));
                fileExists = true;
            }
            catch (FileNotFoundException)
            {
                // File doesn't exist
                fileExists = false;
            }

            if (!fileExists)
            {
                // Initialise the return data
                theData = string.Empty;
            }
            else
            {
                // File does exists, so open it and read the contents
                Stream readStream = await storageFile.OpenStreamForReadAsync();
                using (StreamReader reader = new StreamReader(readStream))
                {
                    theData = await reader.ReadToEndAsync();
                }
            }

            return theData;
        }

        public static async void getReadSettings()
        {
            string savedSettings = string.Empty;

            // There's no FileExists method in WinRT, so have to try to get a reference to it
            // and catch the exception instead
            StorageFile storageFile = null;
            bool fileExists = false;
            try
            {
                // See if file exists
                storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appdata:///local/" + READ_SETTING));
                fileExists = true;
            }
            catch (FileNotFoundException)
            {
                // File doesn't exist
                fileExists = false;
            }

            if (!fileExists)
            {
                // Initialize the return data and save a copy to file...
                App.ReadSettings = new readSettings();
                App.ReadSettings.getCurrentLatLonRage();
                saveReadSettings();
            }
            else
            {
                // File does exists, so open it and read the contents
                Stream readStream = await storageFile.OpenStreamForReadAsync();
                using (StreamReader reader = new StreamReader(readStream))
                {
                    savedSettings = await reader.ReadToEndAsync();
                    readSettings temp = JsonConvert.DeserializeObject<readSettings>(savedSettings.ToString());
                    reader.Close();
                    if (temp != null && temp.getVersion() == App.ReadSettings.getVersion())
                        App.ReadSettings = temp;
                    else
                    {
                        App.ReadSettings = new readSettings();
                        App.ReadSettings.getCurrentLatLonRage();
                        saveReadSettings();
                    }
                }
            }
        }

        public static async void saveReadSettings()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create the file in the local folder, or if it already exists, just open it
            Windows.Storage.StorageFile storageFile =
                await localFolder.CreateFileAsync(READ_SETTING, CreationCollisionOption.ReplaceExisting);

            Stream writeStream = await storageFile.OpenStreamForWriteAsync();
            using (StreamWriter writer = new StreamWriter(writeStream))
            {
                string savedSettings = JsonConvert.SerializeObject(App.ReadSettings);

                await writer.WriteAsync(savedSettings);
                writer.Close();
            }
        }

        // Get a reference to the Mutex. 
        private static Mutex mut = new Mutex(false, "MsgLogMutex");

        public static void loadMsgList()
        {
            string savedMsgs = string.Empty;

            mut.WaitOne(); // Wait until it is safe to enter

            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream(MSG_GROUP_LIST, FileMode.Open, myStore))
                {
                    // Read the data.
                    using (var isoFileReader = new StreamReader(isoFileStream))
                    {
                        savedMsgs = isoFileReader.ReadToEnd();
                        MessageGroup temp = JsonConvert.DeserializeObject<MessageGroup>(savedMsgs.ToString());
                        isoFileReader.Close();
                        if (temp != null)
                            App.ReadMsgList = temp;
                        else
                        {
                            App.ReadMsgList = new MessageGroup();
                        }
                    }
                }
            }
            catch
            {
                App.ReadMsgList = new MessageGroup();
            }
            finally
            {
                mut.ReleaseMutex(); // Release the Mutex. 
            }
        }

        public static void saveMsgList()
        {
            mut.WaitOne(); // Wait until it is safe to enter

            try
            {
                // Obtain the virtual store for the application.
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream(MSG_GROUP_LIST, FileMode.Create, myStore))
                {
                    //Write the data
                    using (var isoFileWriter = new StreamWriter(isoFileStream))
                    {
                        string savedMsgs = JsonConvert.SerializeObject(App.ReadMsgList);
                        isoFileWriter.WriteLine(savedMsgs);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                mut.ReleaseMutex(); // Release the Mutex. 
            }
        }
    }
}
