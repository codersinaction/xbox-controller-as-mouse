using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBoxAsMouse
{
	class Program
	{
		static void Main(string[] args)
		{
			var inputMonitor = new XBoxControllerAsMouse();
			inputMonitor.Start();
			Console.WriteLine("XBox Controller to Mouse has started");
			Console.ReadLine();
		}
	}
}
