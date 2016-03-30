using System;
using System.Diagnostics;
using System.IO; 

namespace TestProject
{

	public class Program
	{
<<<<<<< HEAD
        public TimeSpan UpTime
        {
            get
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();       //Call this an extra time before reading its value
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }
        }
        static void Main()
        {
            int ticks = System.Environment.TickCount/1000;
            Console.WriteLine(ticks);
            Console.ReadKey();
        }
=======
		static void Main()
		{
			var uptimetext1 = File.ReadAllText (@"/proc/uptime"); 
			Console.WriteLine (uptimetext1);
			var uptimetext2 = File.ReadAllText (@"/proc/loadavg"); 
			Console.WriteLine (uptimetext2);
			Console.ReadKey ();
		}
>>>>>>> 53092711e1d60361df5650c4f258c87a2bd65216
	}
}
