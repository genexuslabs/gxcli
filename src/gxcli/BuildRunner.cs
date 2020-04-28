using System;
using System.Collections.Generic;
using System.IO;
using common;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace gxcli
{
	internal class BuildRunner
	{
		public static void Execute(ConfigProvider provider, Dictionary<string, string> props)
		{
			ValidateRequiredProperties(provider, props);

			List<ILogger> loggers = new List<ILogger>
			{
				new ConsoleLogger(LoggerVerbosity.Normal)
			};

			props["GX_PROGRAM_DIR"] = AppDomain.CurrentDomain.BaseDirectory;

			string scriptPath = provider.AssemblyLocation.Replace(".dll", ".targets");
			if (!File.Exists(scriptPath))
			{
				scriptPath = provider.AssemblyLocation.Replace(".dll", ".msbuild");
				if (!File.Exists(scriptPath))
					throw new FileNotFoundException("Could not find MSBuild script", scriptPath);
			}

			BuildRequestData requestData = new BuildRequestData(scriptPath, props, "4.0", new string[] { provider.Target }, null);

			var parameters = new BuildParameters(new ProjectCollection())
			{
				Loggers = loggers
			};

			BuildResult result = BuildManager.DefaultBuildManager.Build(parameters, requestData);

			if (result.Exception != null)
				throw result.Exception;
		}

		private static void ValidateRequiredProperties(ConfigProvider provider, Dictionary<string, string> props)
		{
			foreach (VerbParameter param in provider.Parameters)
			{
				if (!param.Optional && !props.ContainsKey(param.Name))
					throw new MissingFieldException($"Missing required parameter:{param.Name}");
			}
		}
	}
}
