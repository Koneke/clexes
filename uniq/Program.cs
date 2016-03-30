namespace uniq
{
	using System;
	using System.Collections.Generic;

	class Program
	{
		static void Main()
		{
			var input = new List<string>();

			if (Console.IsInputRedirected)
			{
				string line;
				while ((line = Console.ReadLine()) != null)
				{
					input.Add(line);
				}
			}

			var set = new HashSet<string>(input);

			foreach (var line in set)
			{
				Console.WriteLine(line);
			}
		}
	}
}