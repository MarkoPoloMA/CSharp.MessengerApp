using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp
{
	public class Repository : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string UserName { get; set; }
		public string Message { get; set; }
		private ObservableCollection<string> messageList;


		public ObservableCollection<string> MessageList
		{
			get
			{
				return messageList;
			}
			set
			{
				messageList = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageList)));
			}
		}
		public Repository()
		{
			messageList = new ObservableCollection<string>();
			this.Message = string.Empty;
			this.UserName = string.Empty;
		}
	}
}
