using System;

namespace XBoxAsMouse
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var inputMonitor = new ControllerToMouseProxy();
			inputMonitor.Start();
			Console.ReadLine();
		}
	}
}
