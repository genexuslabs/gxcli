using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace gxcli
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				List <ILogger> loggers = new List<ILogger>
				{
					new ConsoleLogger(Microsoft.Build.Framework.LoggerVerbosity.Detailed)
				};

				Dictionary<string, string> props = new Dictionary<string, string>()
				{
					{ "GX_PROGRAM_DIR" , @"C:\code\genexus\trunk\Deploy\GeneXus\Debug" },
					{ "WorkingDirectory", @"C:\GXmodels\junk\AzureDeployTest" }

				};

				BuildRequestData requestData = new BuildRequestData(@"C:\code\genexus\gxcli\res\General.msbuild", props, null, new string[] { "Build" }, null, BuildRequestDataFlags.ProvideProjectStateAfterBuild | BuildRequestDataFlags.ProvideSubsetOfStateAfterBuild);


				var pc = new ProjectCollection();

				var parameters = new BuildParameters(pc)
				{
					Loggers = loggers
				};

				BuildManager bldMgr = new BuildManager();

				bldMgr.Build(parameters, requestData);
			}
			catch (Exception ex)
			{
				Console.Beep(440, 1);
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
