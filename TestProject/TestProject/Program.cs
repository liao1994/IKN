using System;
using System.Diagnostics;

namespace TestProject
{

	class Runshell
	{
		static void Main()
		{
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = "/bin/bash/";
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;

			psi.Arguments = "test";
			Process p = Process.Start(psi);
			string strOutput = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			Console.WriteLine(strOutput);
		}
	}
}
