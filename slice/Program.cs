namespace slice
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

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

			var indexes = new List<int>();
			int index;

			while (Int32.TryParse(arguments[0], out index))
			{
				indexes.Add(index);
				arguments.RemoveAt(0);
			}

			foreach (var line in arguments)
			{
				var sliced = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				var output = new List<string>();

				foreach (var i in indexes)
				{
					output.Add(sliced[i]);
				}

				Console.WriteLine(string.Join(" ", output));
			}
		}
	}
}