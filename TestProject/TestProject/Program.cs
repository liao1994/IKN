using System;
using System.Diagnostics;
using System.IO; 

namespace TestProject
{

	class Runshell
	{
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
	}
}
