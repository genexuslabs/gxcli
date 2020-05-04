using System.Collections.Generic;
using common;
using gxcli.common;

namespace Build
{
	public class RunProvider : IGXCliVerbProvider
	{
		public string Name => "run";

		public string Description => "Builds one object and the ones it calls for the working environment, and executes it.";

		public string Target => "Run";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ObjectName", Description = "Object to execute.", Required = true },
			new VerbParameter { Name = "ForceRebuild", Description = "Force rebuild the object." },
			new VerbParameter { Name = "BuildCalled", Description = "Build only the Object if false, or the Object and all the called if true (Default)." },
			new VerbParameter { Name = "DetailedNavigation", Description = "Show detailed navigation." },
			new VerbParameter { Name = "Parameters", Description = "Parameters for the execution." },
			new VerbParameter { Name = "Build", Description = "If true, (default) executes build before running." }
		};

		public List<Example> Examples => new List<Example>
		{
			new Example { Command = "gx run kbpath=C:\\Models\\MyKB ObjectName=MyMainWebPanel", Description = "Builds an object called MyMainWebPanel from MyKB Knowledge Base" }
		};
	}
}
