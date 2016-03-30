using System;
using System.Collections.Generic;
using System.Linq;

namespace trim
{
	class Program
	{
		static void Main(string[] args)
		{
			var arguments = new List<string>(args);
			var switches = new List<string>(arguments.Where(a => a[0] == '-'));
			arguments.RemoveAll(a => switches.Contains(a));

			if (Console.IsInputRedirected)
			{
				string line;
				while (!string.IsNullOrEmpty(line = Console.ReadLine()))
				{
					arguments.Add(line);
				}
			}

			if (!arguments.Any())
			{
				//PrintHelp();
				return;
			}

			var toTrim = switches.FirstOrDefault(s => s.StartsWith("-s"));

			if (toTrim == null)
			{
				return;
			}

			toTrim = toTrim.Replace("-s", "");
			toTrim = toTrim.Replace("quote", "\"");

			foreach (var line in arguments)
			{
				Console.WriteLine(line.Trim(toTrim.ToCharArray()));
			}
		}
	}
}
