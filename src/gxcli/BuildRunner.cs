using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using common;
using gxcli.common;
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
			if (provider.HasValidator)
				ValidateVerbSpecificParameters(provider, props);

			List<ILogger> loggers = new List<ILogger>
			{
				new ConsoleLogger(GetLoggerVerbosity(props))
			};

			props["GX_PROGRAM_DIR"] = Config.Default.GeneXusPath;

			string scriptPath = provider.AssemblyLocation.Replace(".dll", ".targets");
			if (!File.Exists(scriptPath))
			{
				scriptPath = provider.AssemblyLocation.Replace(".dll", ".msbuild");
				if (!File.Exists(scriptPath))
					throw new FileNotFoundException("Could not find MSBuild script", scriptPath);
			}

			string[] targets = provider.Target.Split(new[] { ',' });
			foreach (string target in targets)
			{
				BuildRequestData requestData = new BuildRequestData(scriptPath, props, "4.0", new string[] { target }, null);

				var parameters = new BuildParameters(new ProjectCollection())
				{
					Loggers = loggers
				};

				BuildResult result = BuildManager.DefaultBuildManager.Build(parameters, requestData);

				if (result.Exception != null)
					throw result.Exception;

				if (result.OverallResult == BuildResultCode.Failure)
					return;

				TargetResult tResult = result.ResultsByTarget[target];
				foreach (ITaskItem item in tResult.Items)
				{
					string[] outProps = item.ItemSpec.Split(new[] { ',' });
					foreach (string outProp in outProps)
					{
						string[] keyVal = outProp.Split(new[] { '=' });
						if (keyVal.Length == 0 || keyVal.Length > 2)
							continue;

						if (keyVal.Length == 2)
							props[keyVal[0]] = keyVal[1];
						else if (keyVal.Length == 1)
							props[keyVal[0]] = bool.TrueString;
					}
				}
			}
		}

		private static LoggerVerbosity GetLoggerVerbosity(Dictionary<string, string> props)
		{
			if (!props.ContainsKey("verbosity"))
				return LoggerVerbosity.Normal;

			string logger = props["verbosity"];
			switch (logger.ToLower())
			{
				case "q":
				case "quiet": 
					return LoggerVerbosity.Quiet;
				case "m":
				case "minimal": 
					return LoggerVerbosity.Minimal;
				case "d": 
				case "detailed": 
					return LoggerVerbosity.Detailed;
				case "diag":
				case "diagnostic": 
					return LoggerVerbosity.Diagnostic;
				default:
					return LoggerVerbosity.Normal;
			}
		}

		private static void ValidateRequiredProperties(ConfigProvider provider, Dictionary<string, string> props)
		{
			foreach (VerbParameter param in provider.Parameters)
			{
				if (param.Required && !props.ContainsKey(param.Name))
					throw new MissingFieldException($"Missing required parameter:{param.Name}");
			}
		}

		private static void ValidateVerbSpecificParameters(ConfigProvider provider, Dictionary<string, string> props)
		{
			Assembly ass = Assembly.LoadFrom(provider.AssemblyLocation);
			Type t = ass.GetType(provider.ClassName);

			if (t != null)
			{
				ParameterValidator validator = Activator.CreateInstance(t) as ParameterValidator;
				validator.Validate(props);
			}
		}
	}
}
