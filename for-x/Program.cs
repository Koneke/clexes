namespace for_x
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

	class Program
	{
		static void Main(string[] args)
		{
			var arguments = new List<string>(args);
			var switches = arguments.Where(x => x[0] == '-').ToList();
			arguments = arguments.Where(x => !switches.Contains(x)).ToList();
			var input = new List<string>();

			if (Console.IsInputRedirected)
			{
				string line;
				while ((line = Console.ReadLine()) != null)
				{
					input.Add(line);
				}
			}

			var fileToken = "$";

			var tokenSwitch = switches.FirstOrDefault(x => x.StartsWith("-t"));
			if (tokenSwitch != null)
			{
				fileToken = tokenSwitch.Substring(2, tokenSwitch.Length - 2);
			}

			if (!arguments.Contains(fileToken))
			{
				arguments.Add(fileToken);
			}

			var commandString = string.Join(" ", arguments);

			foreach (var line in input)
			{
				var l = line;

				if (switches.Contains("-q"))
				{
					l = line.Trim('"');
					l = $"\"{l}\"";
				}

				var p = new Process
				{
					StartInfo = new ProcessStartInfo("cmd", "/c " + commandString.Replace(fileToken, l))
					{
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true
					}
				};
				p.Start();

				var result = p.StandardOutput.ReadToEnd();
				Console.Write(result);
			}
		}
	}
}