using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerApp
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static Client tcpClient = new Client();
		//private static string message = string.Empty;
		static bool connection = true;
		public MainWindow(string loginUser)
		{
			InitializeComponent();
			Repository repos = new Repository();
			this.DataContext = repos;
			repos.UserName = loginUser;
			login.Content= loginUser;
            repos.Message = ($"connected");
            TcpConnection(repos);

            //InputMessageTextBox.IsEnabled = true;
            //DisconnectionButton.IsEnabled = true;

        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
		{
			if (InputMessageTextBox.Text.Length > 0)
			{
				var repos = this.DataContext as Repository;
				repos.Message = InputMessageTextBox.Text;
				InputMessageTextBox.Text = string.Empty;
			}
		}
		private static async void TcpConnection(Repository repos)
		{
            await Task.Run(() =>
			{
				try
				{
					TcpClient client = new TcpClient("127.0.0.1", 8080);
					
					Task.Run(() =>
					{
						while (true)
						{   NetworkStream stream = client.GetStream();
        
							try
							{
								byte[] buffer = new byte[256];
								int size = stream.Read(buffer, 0, buffer.Length);
								if (size > 0)
								{
									string message = Encoding.UTF8.GetString(buffer, 0, size);
									App.Current.Dispatcher.Invoke(() =>
									{
										repos.MessageList.Add(message);
									});
								}
								if (!connection)
								{
                                    repos.Message = ($"User {repos.UserName} diconected");
                                    stream.Close();
									client.Close();
									break;
								}
							}
							catch (Exception ex)
							{
								MessageBox.Show("Error receiving message: " + ex.Message);
								break;
							}
						}
					});

					while (true)
					{
						NetworkStream stream = client.GetStream();
						if (connection)
						{
							if (!string.IsNullOrEmpty(repos.Message))
							{
								byte[] data = Encoding.UTF8.GetBytes($"[{DateTime.Now:HH:mm} {repos.UserName}] {repos.Message}");
								stream.Write(data, 0, data.Length);
								stream.Flush();

								repos.Message = string.Empty;
							}
						}
						if (!connection)
						{
                            //byte[] data = Encoding.UTF8.GetBytes($"{repos.UserName} disconected");
                            //stream.Write(data, 0, data.Length);

                            stream.Close();
							client.Close();
							break;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Connection error: " + ex.Message);
				}
			});
		}
		//private void ConnectionButton_Click(object sender, RoutedEventArgs e)
		//{

		//	if (LoginTextBox.Text.Length > 0)
		//	{
		//		connection = true;
		//		var repos = DataContext as Repository;
		//		repos.UserName = LoginTextBox.Text;
		//		ConnectionButton.IsEnabled = false;
		//		InputMessageTextBox.IsEnabled = true;
		//		DisconnectionButton.IsEnabled = true;
		//		LoginTextBox.IsEnabled = false;
  //              repos.Message = ($"User {repos.UserName} connected");

  //              TcpConnection(repos);
		//	}
		//	else
		//		MessageBox.Show("Enter your logon");
		//}

		private void DisconnectionButton_Click(object sender, RoutedEventArgs e)
		{
            var repos = DataContext as Repository;
            repos.Message = ($"disconected");

			connection = false;
    
			//LoginTextBox.IsEnabled = true;
			DisconnectionButton.IsEnabled = false;
			InputMessageTextBox.Text = string.Empty;
			InputMessageTextBox.IsEnabled = false;
			Close();

        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
		}
	}
}
