using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_client
{
	class Program
	{
		static void Main(string[] args)
		{
			string str = " ";
			Console.WriteLine ("client starts..");
			byte[] packetData;
			ConsoleKeyInfo cki;
			byte[] bytesFromServer = new byte[1000]; 
			const string IPadress = "192.168.217.130";
			const int port = 9000;
			var ep = new IPEndPoint(IPAddress.Any, 0);
			var udpclient = new UdpClient (IPadress,9000);
			var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			string returnData = " ";
			Console.WriteLine("(Q)uit, (U)ptime, (L)oadavg");
			do
			{
				packetData = Encoding.ASCII.GetBytes("0");
				cki = Console.ReadKey();
				if(cki.Key == ConsoleKey.U){
					packetData = Encoding.ASCII.GetBytes("U");
					str = "U";
				}
				//proc/uptime

				if (cki.Key == ConsoleKey.L)
				{
					packetData = Encoding.ASCII.GetBytes("L");
					str = "L";
				}
				//proc/loadavg
				udpclient.Send(packetData,str.Length);

				var recv = udpclient.Receive(ref ep);
				var s2 = Encoding.ASCII.GetString(recv);
				Console.WriteLine(s2);
			

					
			} while (cki.Key != ConsoleKey.Q && cki.Key != ConsoleKey.Escape);
			udpclient.Close ();




		}

	}


}
