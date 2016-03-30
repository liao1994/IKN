using System;
using System.Diagnostics;
using System.IO; 

namespace TestProject
{

	public class Program
	{
		static void Main()
		{
			var uptimetext1 = File.ReadAllText (@"/proc/uptime"); 
			Console.WriteLine (uptimetext1);
			var uptimetext2 = File.ReadAllText (@"/proc/loadavg"); 
			Console.WriteLine (uptimetext2);
			Console.ReadKey ();
		}
	}
}
