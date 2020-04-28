using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using common;
using gxcli.common;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace gxcli
{
	class Program
	{
		const string INSTALL = "install";
		const string HELP = "help";

		static void Main(string[] args)
		{
			try
			{
				if (args.Length == 0)
				{
					ShowUsage();
					return;
				}

				string verb = args[0];
				if (verb.Equals(INSTALL, StringComparison.InvariantCultureIgnoreCase))
				{
					PrintHeader();
					Config.Install();
					return;
				}
				if (verb.Equals(HELP, StringComparison.InvariantCultureIgnoreCase))
				{
					ShowUsage();
					return;
				}

				Dictionary<string, string> options = ParseArguments(args);

				if (Config.Default.Providers.ContainsKey(verb.ToLower()))
				{
					ConfigProvider provider = Config.Default.Providers[verb.ToLower()];
					BuildRunner.Execute(provider, options);
					return;
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Exception inner = ex.InnerException;
				while (inner != null)
				{
					Console.WriteLine(inner.Message);
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
					continue;

				string[] keyVal = item.Split(new[] {'='});
				if (keyVal.Length != 2)
					continue;

				parsed[keyVal[0].ToLower()] = keyVal[1];
			}

			return parsed;
		}

		private static void ShowUsage()
		{
			PrintHeader();

			Console.WriteLine("Usage: gx [verb] [options] [global options]");
			Console.WriteLine("");
			Console.WriteLine("Verbs:");
			foreach (string verb in Config.Default.Providers.Keys)
			{
				ConfigProvider provider = Config.Default.Providers[verb];
				Console.WriteLine($" - {provider.Name}: {provider.Description}");
				if (provider.Parameters.Count > 0)
				{
					Console.WriteLine($"\tOptions:");

					foreach (VerbParameter param in provider.Parameters)
					{
						Console.WriteLine($"\t -- {param.Name}{(param.Optional ? " [Optional]" : "")}: {param.Description}");
					}
				}
				Console.WriteLine("");
			}

			Console.WriteLine("Global options:");
			Console.WriteLine("- verbosity: quiet|minimal|normal(default)|detailed|diagnostic");
		}

		private static void PrintHeader()
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			IEnumerable<Attribute> assemblyAtt = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute));
			IEnumerable<Attribute> assemblyCop = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute));
			string title = ((AssemblyTitleAttribute)assemblyAtt.First()).Title;
			string copyRight = ((AssemblyCopyrightAttribute)assemblyCop.First()).Copyright;
			string version = assembly.GetName().Version.ToString();

			Console.WriteLine($"{title} version {version}");
			Console.WriteLine(copyRight);
			Console.WriteLine("");
		}

		

		
	}
}
