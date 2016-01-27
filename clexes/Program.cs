using System.IO;
using System.Text;

namespace clexes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	class Cat
	{
		class CountingReader : StreamReader {
			private int _lineNumber = 0;
			public int LineNumber { get { return _lineNumber; } }

			public CountingReader(Stream stream) : base(stream) { }

			public override string ReadLine() {
				_lineNumber++;
				return base.ReadLine();
			}
		}

		class F
		{
			public string Name { get; set; }
			public string FullPath { get; set; }
		}

		static void PrintHelp()
		{
			Console.WriteLine("Usage cat [-f] [-n] [-l] file1 file2 .. fileN");
			Console.WriteLine("(or as piped input)");
			Console.WriteLine("-f : show full path of the file before each line.");
			Console.WriteLine("-n : show file name before each line.");
			Console.WriteLine("-l : show line number before each line.");
		}

		static void Main(string[] args)
		{
			var arguments = new List<string>(args);
			var switches = new List<string>(arguments.Where(a => a[0] == '-'));
			arguments.RemoveAll(a => switches.Contains(a));

			if (!arguments.Any())
			{
				PrintHelp();
				return;
			}

			if (Console.IsInputRedirected)
			{
				string line;
				while (!string.IsNullOrEmpty(line = Console.ReadLine()))
				{
					arguments.Add(line);
				}
			}

			List<F> files = new List<F>();

			try
			{
				foreach (var arg in arguments)
				{
					files.Add(new F{
						Name = Path.GetFileName(arg).Trim(),
						FullPath = Path.GetFullPath(arg).Trim()
					});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}

			var formatters = new List<string>();

			if (switches.Contains("-f"))
			{
				var longest = files.Max(f => f.FullPath.Length);
				Console.WriteLine("longest f {0}", longest);
				formatters.Add("{2," + longest + "}");
			}

			if (switches.Contains("-n"))
			{
				var longest = files.Max(f => f.Name.Length);
				Console.WriteLine("longest n {0}", longest);
				formatters.Add("{1," + longest + "}");
			}

			if (switches.Contains("-l"))
			{
				formatters.Add("({3})");
			}

			var formatString = string.Join(" ", formatters);

			if (formatters.Any())
			{
				formatString += ": ";
			}

			formatString += "{0}";

			foreach(var f in files)
			{
				using (var stream = new CountingReader(new FileStream(f.FullPath, FileMode.Open, FileAccess.Read)))
				{
					string line;
					while ((line = stream.ReadLine()) != null)
					{
						Console.WriteLine(
							formatString,
							line,
							f.Name,
							f.FullPath,
							stream.LineNumber);
					}
				}
			}
		}
	}
}