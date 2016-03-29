﻿using System;
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
			byte[] packetData;
			ConsoleKeyInfo cki;

			const string IPadress = "192.168.217.130";
			const int port = 9000;
			var ep = new IPEndPoint(IPAddress.Parse(IPadress), port);
			var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			Console.WriteLine("(Q)uit, (U)ptime, (L)oadavg");
			do
			{
				packetData = Encoding.ASCII.GetBytes("0");
				cki = Console.ReadKey();
				if(cki.Key == ConsoleKey.U)
					packetData = Encoding.ASCII.GetBytes("U");
				//proc/uptime
				if (cki.Key == ConsoleKey.L)
					packetData = Encoding.ASCII.GetBytes("L");
				//proc/loadavg

				client.SendTo(packetData, ep);


			} while (cki.Key != ConsoleKey.Q && cki.Key != ConsoleKey.Escape);







		}

	}


}
