using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using localChat.Resources;


namespace localChat
{
    public class MessageGroup : INotifyPropertyChanged
    {
        public MessageGroup()
        {
            this.Items = new ObservableCollection<MessageItem>();

            /* Get saved msgs.. */

        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MessageItem> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public int CurrentItemCount()
        {
            return this.Items.Count;
        }

        public int getCurMsgIndex(string curMsgId)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].dbMsgID.Equals(curMsgId))
                    return i;
            }
            return -1;
        }

        public bool isInRange(MessageItem curMsg)
        {
            if ((App.ReadSettings.latStart <= curMsg.Lat) && (curMsg.Lat <= App.ReadSettings.latEnd) &&
                (App.ReadSettings.lonStart <= curMsg.Lon) && (curMsg.Lon <= App.ReadSettings.lonEnd))
                return true;
                        
            return false;
        }

        public bool msgInsert(MessageItem curMsg)
        {
            /* Check to see if the message is in our range.. */
            if (!isInRange(curMsg))
                return false;

            if (this.Items.Count == 0)   /* Just insert it.. */
            {
                this.Items.Add(curMsg);
                return true;
            }

            /* Now, try to insert the message in to our list in reverse chronological order */
            /* Assuming the message list is already sorted this way.. */
            for (int i = 0; i < this.Items.Count; i++)
            {
                int result = curMsg.CreateDate.CompareTo(this.Items[i].CreateDate) ;

                if (result > 0) /* Later than the current index one.. */
                {
                    this.Items.Insert(i, curMsg);
                    return true;
                }
                else if (result == 0)   /* Same time.. */
                {
                    /* Same message.. */
                    if (curMsg.dbMsgID.Equals(this.Items[i].dbMsgID))
                    {
                        return false;
                    }
                    else/* Different message but same time.. */
                    {
                        this.Items.Insert(i, curMsg);
                        return true;
                    }
                }
                else /* Earlier than the current index one.. */
                {
                    if (i == this.Items.Count - 1)
                    {
                        /* 
                         * This message is even older than the last message
                         * we have in our list.  So, just add it..
                         */
                        this.Items.Add(curMsg);
                        return true;
                    }
                    else
                    {
                        //move on to the next one..
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            FileStorageOperations.loadMsgList();

            /* Remove message that is too old.. */
            TimeSpan keepDays = new System.TimeSpan(App.ReadSettings.keepTime, 0, 0, 0);

            for (int i = 0; i < App.ReadMsgList.CurrentItemCount();)
            {
                /* Only add message if is newer than what we want to see */
                if (DateTime.Now.Subtract(keepDays).CompareTo(App.ReadMsgList.Items[i].CreateDate) > 0)
                    // msg is too old..
                    App.ReadMsgList.Items.Remove(App.ReadMsgList.Items[i]);
                else
                    i++;
            }

            // Sample data; replace with real data
            //this.Items.Add(new MessageItem() { dbMsgID = "0", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime one", Author = "Maecenas praesent accumsan bibendum", Msg = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new MessageItem() { dbMsgID = "1", Date = "12/25/2012", Time = "2:30:45PM", Title = "runtime two", Author = "Dictumst eleifend facilisi faucibus", Msg = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new MessageItem() { dbMsgID = "2", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime three", Author = "Habitant inceptos interdum lobortis", Msg = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new MessageItem() { dbMsgID = "3", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime four", Author = "Nascetur pharetra placerat pulvinar", Msg = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new MessageItem() { dbMsgID = "4", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime five", Author = "Maecenas praesent accumsan bibendum", Msg = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new MessageItem() { dbMsgID = "5", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime six", Author = "Dictumst eleifend facilisi faucibus", Msg = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });

            App.ReadMsgList.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}