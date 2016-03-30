namespace take
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	class Program
	{
		static void Main(string[] args)
		{
			var arguments = new List<string>(args);
			var input = new List<string>();

			if (Console.IsInputRedirected)
			{
				string line;
				while ((line = Console.ReadLine()) != null)
				{
					input.Add(line);
				}
			}

			if (!arguments.Any())
			{
				//PrintHelp();
				return;
			}

			var fromEnd = false;
			if (arguments.Contains("-e"))
			{
				arguments.Remove("-e");
				fromEnd = true;
			}

			int take;
			if (!int.TryParse(arguments[0], out take))
			{
				return;
			}
			arguments.RemoveAt(0);

			var tempList = new List<string>(input);

			if (fromEnd)
			{
				tempList.Reverse();
			}

			if (take < 0)
			{
				take = tempList.Count + take;
			}

			tempList =
				//take < 0 ? tempList.Skip(tempList.Count + take).ToList() :
				tempList.Take(take).ToList();

			if (fromEnd)
			{
				tempList.Reverse();
			}

			foreach (var line in tempList)
			{
				Console.WriteLine(line);
			}
		}
	}
}