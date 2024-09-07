using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	public class Program
	{
		static void Main(string[] args)
		{
			const int port = 8080;
			const string ip = "127.0.0.1";
			TcpListener serverSocket = new TcpListener(IPAddress.Parse(ip), port);
			serverSocket.Start();
			Console.WriteLine("Server started.");

			List<TcpClient> СonnectedClients = new List<TcpClient>();

			try
			{
				while (true)
				{
					TcpClient clientSocket = serverSocket.AcceptTcpClient();
					lock (СonnectedClients)
					{
						СonnectedClients.Add(clientSocket);
					}
					Console.WriteLine("Client connected!");
				
					
					Task.Run(() => HandleClient(clientSocket, СonnectedClients));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Server error: " + ex.Message);
			}
			finally
			{
				serverSocket.Stop();
				Console.WriteLine("Server stopped.");
			}

			void HandleClient(TcpClient clientSocket, List<TcpClient> connectedClients)
			{
				NetworkStream stream = clientSocket.GetStream();

				try
				{
					while (true)
					{
						byte[] buffer = new byte[256];
						int size = stream.Read(buffer, 0, buffer.Length);
						if (size == 0) break;
						string data = Encoding.UTF8.GetString(buffer, 0, size);

						Console.WriteLine("Received: " + data);

						
						BroadcastMessage(data, connectedClients);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("Client disconnected: " + e.Message);
				}
				finally
				{
					lock (connectedClients)
					{
						connectedClients.Remove(clientSocket);
					}
					clientSocket.Close();
				}
			}

			void BroadcastMessage(string message, List<TcpClient> connectedClients)
			{
				byte[] data = Encoding.UTF8.GetBytes(message);

				lock (connectedClients)
				{
					foreach (var client in connectedClients)
					{
						try
						{
							NetworkStream stream = client.GetStream();
							stream.Write(data, 0, data.Length);
							stream.Flush();
						}
						catch (Exception)
						{
						}
					}
				}
			}




			/*//ТЕСТОВЫЙ КОД
			const int port = 8080;
			TcpListener serverSocket = new TcpListener(IPAddress.Any, port);
			serverSocket.Start();
			Console.WriteLine("Server started.");

			try
			{
				while (true)
				{
					TcpClient clientSocket = serverSocket.AcceptTcpClient();
					Console.WriteLine("Client connected!");

					NetworkStream stream = clientSocket.GetStream();

					try
					{
						while (true)
						{
							byte[] buffer = new byte[256];
							int size = stream.Read(buffer, 0, buffer.Length);
							string data = Encoding.UTF8.GetString(buffer, 0, size);


							Console.WriteLine("Client: " + data);


							byte[] response = Encoding.UTF8.GetBytes(data);
							stream.Write(response, 0, response.Length);
							stream.Flush();
						}
					}
					catch (Exception e)
					{
						Console.WriteLine("Client disconnected: " + e.Message);
						clientSocket.Close();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Server error: " + ex.Message);
			}
			finally
			{
				serverSocket.Stop();
				Console.WriteLine("Server stopped.");
			}*/
		}
	}
}
