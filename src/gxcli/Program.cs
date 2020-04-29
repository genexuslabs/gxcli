using System;
using System.Collections.Generic;
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
					Config.Install();
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

				Dictionary<string, string> options = ParseArguments(args);

				if (Config.Default.Providers.ContainsKey(verb.ToLower()))
				{
					if (options.ContainsKey(HELP))
					{
						OutputHelper.ShowUsage(verb);
						return;
					}

					ConfigProvider provider = Config.Default.Providers[verb.ToLower()];
					BuildRunner.Execute(provider, options);
					return;
				}

			}
			catch (Exception ex)
			{
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
			}
		}

		private static Dictionary<string, string> ParseArguments(string[] args)
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
