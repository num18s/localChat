﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace localChat
{
    public class MessageItem : INotifyPropertyChanged
    {
        private string _dbMsgId;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string dbMsgID
        {
            get
            {
                return _dbMsgId;
            }
            set
            {
                if (value != _dbMsgId)
                {
                    _dbMsgId = value;
                    NotifyPropertyChanged("dbMsgID");
                }
            }
        }

        private string _title;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _author;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                if (value != _author)
                {
                    _author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

        private string _date;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    NotifyPropertyChanged("Date");
                }
            }
        }

        private string _time;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        private string _msg;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Msg
        {
            get
            {
                return _msg;
            }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    NotifyPropertyChanged("Msg");
                }
            }
        }

        public float _lat;
        public float Lat
        {
            get
            {
                return _lat;
            }
            set
            {
                if (value != _lat)
                {
                    _lat = value;
                    NotifyPropertyChanged("Lat");
                }
            }
        }
        public float _lon;
        public float Lon
        {
            get
            {
                return _lon;
            }
            set
            {
                if (value != _lon)
                {
                    _lon = value;
                    NotifyPropertyChanged("Lon");
                }
            }
        }

        public DateTime _createDate;
        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                if (value != _createDate)
                {
                    _createDate = value;
                    NotifyPropertyChanged("Lon");
                }
            }
        }
        
        private bool _showLocation;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public bool ShowLocation
        {
            get
            {
                return _showLocation;
            }
            set
            {
                if (value != _showLocation)
                {
                    _showLocation = value;
                    NotifyPropertyChanged("ShowLocation");
                }
            }
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