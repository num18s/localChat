﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.AlertData
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class AlertData { }
#else

	public class AlertData : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public AlertData()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/localChat;component/SampleData/AlertData/AlertData.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private ItemCollection _Collection = new ItemCollection();

		public ItemCollection Collection
		{
			get
			{
				return this._Collection;
			}
		}
	}

	public class Item : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _Message = string.Empty;

		public string Message
		{
			get
			{
				return this._Message;
			}

			set
			{
				if (this._Message != value)
				{
					this._Message = value;
					this.OnPropertyChanged("Message");
				}
			}
		}

		private string _MessageTitle = string.Empty;

		public string MessageTitle
		{
			get
			{
				return this._MessageTitle;
			}

			set
			{
				if (this._MessageTitle != value)
				{
					this._MessageTitle = value;
					this.OnPropertyChanged("MessageTitle");
				}
			}
		}

		private string _MessageDate = string.Empty;

		public string MessageDate
		{
			get
			{
				return this._MessageDate;
			}

			set
			{
				if (this._MessageDate != value)
				{
					this._MessageDate = value;
					this.OnPropertyChanged("MessageDate");
				}
			}
		}

		private string _MessageTime = string.Empty;

		public string MessageTime
		{
			get
			{
				return this._MessageTime;
			}

			set
			{
				if (this._MessageTime != value)
				{
					this._MessageTime = value;
					this.OnPropertyChanged("MessageTime");
				}
			}
		}
	}

	public class ItemCollection : System.Collections.ObjectModel.ObservableCollection<Item>
	{ 
	}
#endif
}
