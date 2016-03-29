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
	class Runnshell
	{
		public Runnshell(string arg)
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = "/Desktop/bash.sh",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				Arguments = arg
			};

			Process p = Process.Start(psi);
			string strOutput = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			Console.WriteLine(strOutput);
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			var data = new byte[1000];
			// lytter til alle IP adresser i port 9000
			var endpoint = new IPEndPoint(IPAddress.Any,9000);
			// storing connect from client 
			var newSocket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
			//bind inc connection to the socket
			newSocket.Bind(endpoint);
			Console.WriteLine("Waiting for client");

			//waiting for connection  from any IP on port 9000
			var sender = new IPEndPoint(IPAddress.Any, 9000);
			//stores connection to tmpRemote
			var tmpRemote = (EndPoint) sender;

			// the stored data in data and size in recv
			var recv = newSocket.ReceiveFrom(data, ref tmpRemote);

			Console.WriteLine("message received from" + tmpRemote);
			//convert the byte array into string
			var datafromClient = Encoding.ASCII.GetString(data, 0, recv);
			Console.WriteLine(datafromClient);
			Runnshell rs;
			switch (datafromClient)
			{
			case "U":
				rs = new Runnshell("proc/uptime");
				//use /proc/uptime
				break;
			case "L":
				rs = new Runnshell("proc/loadavg");
				// use /proc/loadavg
				break;
			default:
				// dunno what to do
				break;
			}
			string welcome = "welcome to my super server!, which is not copy/pasted";
			data = Encoding.ASCII.GetBytes(welcome);

			if (newSocket.Connected)
				newSocket.Send(data);
			while (true)
			{
				if (!newSocket.Connected)
				{
					Console.WriteLine("Client disconnected");
					break;
				}
				data = new byte[1000];
				recv = newSocket.ReceiveFrom(data, ref tmpRemote);
				if (recv == 0)
					break;
				Console.WriteLine(Encoding.ASCII.GetString(data,0,recv));
			}
			newSocket.Close();
		}
	}
}
