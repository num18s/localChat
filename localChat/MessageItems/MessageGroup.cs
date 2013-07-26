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

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            //this.Items.Add(new MessageItem() { ID = "0", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime one", Author = "Maecenas praesent accumsan bibendum", Msg = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new MessageItem() { ID = "1", Date = "12/25/2012", Time = "2:30:45PM", Title = "runtime two", Author = "Dictumst eleifend facilisi faucibus", Msg = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new MessageItem() { ID = "2", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime three", Author = "Habitant inceptos interdum lobortis", Msg = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new MessageItem() { ID = "3", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime four", Author = "Nascetur pharetra placerat pulvinar", Msg = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new MessageItem() { ID = "4", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime five", Author = "Maecenas praesent accumsan bibendum", Msg = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new MessageItem() { ID = "5", Date = "1/2/2013", Time = "12:53:40AM", Title = "runtime six", Author = "Dictumst eleifend facilisi faucibus", Msg = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            
            this.IsDataLoaded = true;
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