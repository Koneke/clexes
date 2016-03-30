namespace slice
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	class Program
	{
		private static bool InQuotes(string input, int position)
		{
			return input.Substring(0, position).Count(c => c == '"') % 2 != 0;
		}

		private static List<string> SplitWithQuotes(string input, string separator)
		{
			var parts = new List<string>();
			var lastSplit = 0;

			do
			{
				var nextSplit = NextSplit(input, separator, lastSplit + 1);

				if (nextSplit == -1)
				{
					parts.Add(input.Substring(lastSplit, input.Length - lastSplit));
					break;
				}

				parts.Add(input.Substring(lastSplit, nextSplit - lastSplit));
				lastSplit = nextSplit + separator.Length;
			}
			while (true);

			parts = parts.Where(s => !string.IsNullOrEmpty(s.Trim())).ToList();

			return parts;
		}

		private static int NextSplit(string input, string separator, int startIndex = 0)
		{
			var position = input.IndexOf(separator, startIndex, StringComparison.Ordinal);

			while (true)
			{
				if (position == -1)
				{
					return -1;
				}

				if (!InQuotes(input, position))
				{
					return position;
				}

				position = input.IndexOf(separator, position + 1, StringComparison.Ordinal);
			}
		}

		static List<string> Split(string input, bool respectQuotes)
		{
			return respectQuotes
				? SplitWithQuotes(input, " ")
				: input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

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

			while (int.TryParse(arguments[0], out index))
			{
				indexes.Add(index);
				arguments.RemoveAt(0);
			}

			var respectQuotes = !switches.Contains("-nq");

			foreach (var line in arguments)
			{
				//var sliced = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
				var sliced = Split(line, respectQuotes);
				var output = new List<string>();

				foreach (var i in indexes)
				{
					if (i < sliced.Count)
					{
						output.Add(sliced[i].Trim());
					}
				}

				Console.WriteLine(string.Join(" ", output));
			}
		}
	}
}