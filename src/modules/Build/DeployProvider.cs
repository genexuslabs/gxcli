using System.Collections.Generic;
using common;
using gxcli.common;

namespace Build
{
	public class DeployProvider : IGXCliVerbProvider
	{
		public string Name => "deploy";

		public string Description => "Deploy the files involved in an application.";

		public string Target => "Deploy";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ObjectNames", Description = "Objects (and its called tree) to be deployed.", Required = true},
			new VerbParameter { Name = "OutputPath", Description = "Where the project file (.gxdproj) will be created." },
			new VerbParameter { Name = "ApplicationKey", Description = "Application key used to encrypt sensible data." },
			new VerbParameter { Name = "TARGET_JRE", Description = "The JRE version of the target environment (7,8,9)." },
			new VerbParameter { Name = "ProjectName", Description = "The name of the project to be created." }
		};

		public List<Example> Examples => new List<Example>
		{
			new Example { Command = "gx deploy kbpath=C:\\Models\\MyKB ObjectNames=WebPanel1", Description = "Deploy an object called WebPanel1 from the default environment of the MyKB Knowledge Base." },
			new Example { Command = "gx deploy kbpath=C:\\Models\\MyKB ObjectNames=WebPanel1 Environment=\"Java Environment\" ObjectNames=WebPanel1 ProjectName=MyDeploy", Description = "Deploy an object called WebPanel1 from a environment called 'Java Environment' of the MyKB Knowledge Base." },
		};
	}
}
