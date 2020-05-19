using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using gxcli.Misc;

namespace gxcli
{
	class Program
	{
		const string INSTALL = "install";
		const string HELP = "help";
		const string VERSION = "version";

		static void Main(string[] args)
		{
			//Debug.Assert(false, "Wanna attach for debugging purposes?");
			try
			{
				if (args.Length == 0)
				{
					OutputHelper.PrintHeader();
					OutputHelper.ShowUsage();
					return;
				}

				string verb = args[0];
				if (verb.Equals(INSTALL, StringComparison.InvariantCultureIgnoreCase))
				{
					string gxPath = "";
					if (args.Length == 2)
					{
						gxPath = args[1];
					}
					if (string.IsNullOrEmpty(gxPath))
					{
						Console.WriteLine("You must enter the path to a valid GeneXus installation.");
						gxPath = Console.ReadLine();
					}
					Config.Install(gxPath);
					return;
				}
				if (verb.Equals(HELP, StringComparison.InvariantCultureIgnoreCase))
				{
					OutputHelper.ShowUsage();
					return;
				}
				if (verb.Equals(VERSION, StringComparison.InvariantCultureIgnoreCase))
				{
					OutputHelper.PrintHeader();
					return;
				}

				if (Config.Default.Providers.ContainsKey(verb.ToLower()))
				{
					Dictionary<string, string> options = ParseArguments(args.Skip(1));

					if (options.ContainsKey(HELP))
					{
						OutputHelper.ShowUsage(verb);
						return;
					}

					ConfigProvider provider = Config.Default.Providers[verb.ToLower()];
					BuildRunner.Execute(provider, options);
					return;
				}

				ThrowInvalidVerb(verb);

			}
			catch (ApplicationException aex)
			{
				Console.WriteLine(string.Empty);

				string[] lines = aex.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
				for (int i = 0; i < lines.Length; i++)
				{
					if (i == 0)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine(lines[i]);
						Console.ResetColor();
					}
					else
						Console.WriteLine(lines[i]);
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Empty);

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);

				Exception inner = ex.InnerException;
#if DEBUG
				Console.WriteLine(ex.StackTrace);
#endif
				while (inner != null)
				{
					Console.WriteLine(inner.Message);
#if DEBUG
					Console.WriteLine(inner.StackTrace);
#endif
					inner = inner.InnerException;
				}
				Console.ResetColor();
			}
		}

		private static void ThrowInvalidVerb(string param)
		{
			int threshold = param.Length / 2;
			List<string> similar = new List<string>();
			foreach (string verb in Config.Default.Providers.Keys)
			{
				int distance = Levenshtein.DamerauLevenshteinDistance(param.ToLower(), verb, threshold);
				if (distance <= threshold)
					similar.Add(verb);
			}

			StringBuilder msg = new StringBuilder($"gx: '{param}' is not a gx command.{Environment.NewLine}");
			if (similar.Count > 0)
			{
				msg.AppendLine("");
				msg.AppendLine("The most similar commands are");
				foreach (string cmd in similar)
				{
					msg.AppendLine($"\t{cmd}");
				}
			}
			msg.AppendLine("");
			msg.Append("See 'gx help'.");

			throw new ApplicationException(msg.ToString());
		}

		private static Dictionary<string, string> ParseArguments(IEnumerable<string> args)
		{
			Dictionary<string, string> parsed = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			foreach (string item in args)
			{
				if (!item.Contains("="))
				{
					parsed[item.ToLower()] = "true";
					continue;
				}

				string[] keyVal = item.Split(new[] {'='});
				if (keyVal.Length != 2)
					continue;

				parsed[keyVal[0].ToLower()] = keyVal[1];
			}

			return parsed;
		}
	}
}
