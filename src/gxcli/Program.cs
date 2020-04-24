using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
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
		public class Options
		{
			[Option('i', "verbose", HelpText = "Installs the cli")]
			public bool Install { get; set; }
		}

		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed<Options>(o =>
				{
					if (o.Install)
						Install();
				});
		}

		private static void Install()
		{
			var dir = AppDomain.CurrentDomain.BaseDirectory;
			string modulesPath = Path.Combine(dir, "gxclimodules");
			foreach (string dllPath in Directory.GetFiles(modulesPath, "*.dll"))
			{
				Assembly ass = Assembly.LoadFrom(dllPath);
				Attribute att = ass.GetCustomAttribute(typeof(GXCliVerbProviderAttribute));
				if (att == null)
					continue;

				foreach (Type t in ass.GetExportedTypes())
				{
					Console.WriteLine($"{t.FullName} from {t.AssemblyQualifiedName}");
					Console.WriteLine($"IGXCliVerbProvider:{typeof(IGXCliVerbProvider).IsAssignableFrom(t)}");
				}
			}
		}

		static void Build()
		{
			try
			{
				List<ILogger> loggers = new List<ILogger>
				{
					new ConsoleLogger(LoggerVerbosity.Normal)
				};

				Dictionary<string, string> props = new Dictionary<string, string>()
				{
					{ "GX_PROGRAM_DIR" , @"C:\code\genexus\trunk\Deploy\GeneXus\Debug" },
					{ "WorkingDirectory", @"C:\GXmodels\junk\AzureDeployTest" }

				};

				BuildRequestData requestData = new BuildRequestData(@"C:\code\genexus\gxcli\res\General.msbuild", props, "4.0", new string[] { "Build" }, null);

				var projs = new ProjectCollection();
				var parameters = new BuildParameters(projs)
				{
					Loggers = loggers
				};

				BuildResult result = BuildManager.DefaultBuildManager.Build(parameters, requestData);

				if (result.Exception != null)
					throw result.Exception;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				Console.WriteLine("Press <enter> to exit...");
				Console.ReadLine();
			}
		}
	}
}
