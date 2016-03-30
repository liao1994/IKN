using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Remoting.Channels;

namespace UDP_server
{
	class Program
	{
		static void Main(string[] args)
		{
			var bytesToClient = new byte[1000];
			// lytter til alle IP adresser i port 9000
			var endpoint = new IPEndPoint(IPAddress.Any,9000);
			var UdpSocket = new UdpClient (endpoint);
			Console.WriteLine ("Waiting for client...");
			var running = true;
			while (running) {

				//venter på bytes fra vores socket via den endpoint
				var recvBytes = UdpSocket.Receive (ref endpoint); 

				var strFromClient = Encoding.ASCII.GetString (recvBytes);
				//var recv = newSocket.ReceiveFrom(data, ref tmpRemote);
				Console.WriteLine ("A connection found");
				Console.WriteLine(strFromClient);

				//convert the byte array into string
				//var datafromClient = Encoding.ASCII.GetString(data, 0, recv);
				string responseToClient = "havn't recieved anything yet";
				switch (strFromClient) {
				case "U":
					responseToClient = File.ReadAllText (@"/proc/uptime");
				//use /proc/uptime
					break;
				case "L":
					responseToClient = File.ReadAllText (@"/proc/loadavg");
				// use /proc/loadavg
					break;
				default:
					Console.WriteLine ("client choose to close line..");
					running = false; 
				// dunno what to do
					break;
				}
				bytesToClient = Encoding.ASCII.GetBytes (responseToClient);
				UdpSocket.Send (bytesToClient, responseToClient.Length, endpoint);
			}

			UdpSocket.Close();
		}
	}
}
