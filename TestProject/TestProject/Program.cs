using System;
using System.Diagnostics;
using System.IO; 

namespace TestProject
{

	class Runshell
	{
		static void Main()
		{
			Process proc = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = @"/root/Desktop/exo-terminal-emulator.desktop",
					Arguments = "echo bla",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = false
				}
			};
			proc.Start();
			StreamReader reader = proc.StandardOutput;
			string result = reader.ReadToEnd ();
			Console.Write (result);
			string strOutput = proc.StandardOutput.ReadToEnd();
			proc.WaitForExit();

			Console.WriteLine(strOutput);
		}
	}
}
