using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerApp
{
	public class Client
	{
		private static string ip = "127.0.0.1";
		private static int port = 8080;
		public string GetAnswer()
		{
			string answer = string.Empty;
			TcpClient client = new TcpClient("127.0.0.1", 8080);
			//Console.WriteLine("Client connected");
			NetworkStream stream = client.GetStream();


			try
			{
				byte[] bytesRead = new byte[256];
				int length = stream.Read(bytesRead, 0, bytesRead.Length);
				answer = Encoding.UTF8.GetString(bytesRead, 0, length);
				//Console.WriteLine($"server answer: {answer}");
			}
			catch
			{
				//Console.WriteLine("Client connection error");
				client.Close();
			}
			return answer;
		}
		public void SendMessage(string message)
		{
			try
			{
				IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
				Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				tcpSocket.Connect(tcpEndPoint);

				byte[] data = Encoding.UTF8.GetBytes(message);
				tcpSocket.Send(data);

				var buffer = new byte[256];
				var size = 0;
				var answer = new StringBuilder();

				do
				{
					size = tcpSocket.Receive(buffer);
					answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
				} while (tcpSocket.Available > 0);

				tcpSocket.Shutdown(SocketShutdown.Both);
				tcpSocket.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error sending message: {ex.Message}");
			}
			/*IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
			Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			tcpSocket.Connect(tcpEndPoint);




			byte[] data = Encoding.UTF8.GetBytes(message);
			tcpSocket.Send(data);

			var buffer = new byte[256];
			var size = 0;
			var answer = new StringBuilder();

			do
			{
				size = tcpSocket.Receive(buffer);
				answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
			}
			while (tcpSocket.Available > 0);


			tcpSocket.Shutdown(SocketShutdown.Both);
			tcpSocket.Close();*/
		}
	}
}
