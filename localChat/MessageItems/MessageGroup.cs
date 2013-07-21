using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using localChat.Resources;

namespace localChat.ViewModels
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

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new MessageItem() { ID = "0", Date = "1/2/2013", Time = "12:53:40AM", Author = "runtime one", Title = "Maecenas praesent accumsan bibendum", Msg = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new MessageItem() { ID = "1", Date = "1/2/2013", Time = "12:53:40AM", Author = "runtime two", Title = "Dictumst eleifend facilisi faucibus", Msg = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new MessageItem() { ID = "2", Date = "1/2/2013", Time = "12:53:40AM", Author = "runtime three", Title = "Habitant inceptos interdum lobortis", Msg = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new MessageItem() { ID = "3", Date = "1/2/2013", Time = "12:53:40AM", Author = "runtime four", Title = "Nascetur pharetra placerat pulvinar", Msg = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new MessageItem() { ID = "4", Date = "1/2/2013", Time = "12:53:40AM", Author = "runtime five", Title = "Maecenas praesent accumsan bibendum", Msg = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new MessageItem() { ID = "5", Date = "1/2/2013", Time = "12:53:40AM", Author = "runtime six", Title = "Dictumst eleifend facilisi faucibus", Msg = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { ID = "6", Author = "runtime seven", Title = "Habitant inceptos interdum lobortis", Msg = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { ID = "7", Author = "runtime eight", Title = "Nascetur pharetra placerat pulvinar", Msg = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            //this.Items.Add(new ItemViewModel() { ID = "8", Author = "runtime nine", Title = "Maecenas praesent accumsan bibendum", Msg = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new ItemViewModel() { ID = "9", Author = "runtime ten", Title = "Dictumst eleifend facilisi faucibus", Msg = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new ItemViewModel() { ID = "10", Author = "runtime eleven", Title = "Habitant inceptos interdum lobortis", Msg = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new ItemViewModel() { ID = "11", Author = "runtime twelve", Title = "Nascetur pharetra placerat pulvinar", Msg = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new ItemViewModel() { ID = "12", Author = "runtime thirteen", Title = "Maecenas praesent accumsan bibendum", Msg = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new ItemViewModel() { ID = "13", Author = "runtime fourteen", Title = "Dictumst eleifend facilisi faucibus", Msg = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { ID = "14", Author = "runtime fifteen", Title = "Habitant inceptos interdum lobortis", Msg = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { ID = "15", Author = "runtime sixteen", Title = "Nascetur pharetra placerat pulvinar", Msg = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

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