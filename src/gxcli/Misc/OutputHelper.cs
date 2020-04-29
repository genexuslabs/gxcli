using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using common;

namespace gxcli.Misc
{
	internal class OutputHelper
	{
		const int HELP_COL_WIDTH = 30;
		const int HELP_COL_MARGIN = 4;

		public static void ShowUsage(string selectedVerb = null)
		{
			Console.WriteLine("");
			Console.WriteLine($"Usage: gx {(string.IsNullOrEmpty(selectedVerb) ? "[command]" : selectedVerb.ToLower())} [parameters]|help [global options]");
			Console.WriteLine("");
			if (string.IsNullOrEmpty(selectedVerb))
			{
				Console.WriteLine("Here are the base commands:");
				Console.WriteLine("");
			}

			foreach (string verb in Config.Default.Providers.Keys)
			{
				if (!string.IsNullOrEmpty(selectedVerb) && !verb.Equals(selectedVerb, StringComparison.InvariantCultureIgnoreCase))
					continue;

				ConfigProvider provider = Config.Default.Providers[verb];
				Console.WriteLine(GetHelpLine(provider.Name, provider.Description));
				if (string.IsNullOrEmpty(selectedVerb))
					continue;

				if (provider.Parameters.Count > 0)
				{
					Console.WriteLine("");
					Console.WriteLine($"Parameters");

					foreach (VerbParameter param in provider.Parameters)
					{
						Console.WriteLine(GetHelpLine(param.Name, param.Description, param.Required));
					}
				}

				if (provider.Examples.Count > 0)
				{
					Console.WriteLine("");
					Console.WriteLine($"Examples");

					foreach (Example example in provider.Examples)
					{
						Console.WriteLine($"{(string.Concat(Enumerable.Repeat(" ", HELP_COL_MARGIN)))}{example.Description}");
						Console.WriteLine($"{(string.Concat(Enumerable.Repeat(" ", HELP_COL_MARGIN)))}{(string.Concat(Enumerable.Repeat(" ", HELP_COL_MARGIN)))}{example.Command}");
						Console.WriteLine("");
					}
				}
			}
			Console.WriteLine("");
			PrintGlobalOptions();

		}

		public static void PrintHeader()
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			IEnumerable<Attribute> assemblyAtt = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute));
			IEnumerable<Attribute> assemblyCop = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute));
			string title = ((AssemblyTitleAttribute)assemblyAtt.First()).Title;
			string copyRight = ((AssemblyCopyrightAttribute)assemblyCop.First()).Copyright;
			string version = assembly.GetName().Version.ToString();

			Console.WriteLine($"{title} version {version}");
			Console.WriteLine(copyRight);
			Console.WriteLine(GeneXusLogo);
			Console.WriteLine("Welcome to the cool new GeneXus CLI!");
			Console.WriteLine("");
		}

		public static string GeneXusLogo = @"
   ____               ___  __         
  / ___| ___ _ __   ___\ \/ /   _ ___ 
 | |  _ / _ \ '_ \ / _ \\  / | | / __|
 | |_| |  __/ | | |  __//  \ |_| \__ \
  \____|\___|_| |_|\___/_/\_\__,_|___/
                                      ";

		public static void PrintGlobalOptions()
		{
			Console.WriteLine("Global options:");
			Console.WriteLine(GetHelpLine("verbosity", "quiet|minimal|normal(default)|detailed|diagnostic"));
		}

		private static string GetHelpLine(string name, string description, bool special = false)
		{
			int colWidth = HELP_COL_WIDTH;
			int margin = HELP_COL_MARGIN;
			int length = name.Length;
			int spaces = colWidth - margin - length;
			if (spaces < 0)
			{
				margin += spaces;
				spaces = 0;
				if (margin < 0)
					margin = 0;
			}

			return $"{(string.Concat(Enumerable.Repeat(" ", margin)))}{name}{(string.Concat(Enumerable.Repeat(" ", spaces)))}: {description}{(special ? " [Required]" : "")}";
		}

	}
}
