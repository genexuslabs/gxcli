using System.Collections.Generic;
using common;
using gxcli.common;

namespace Build
{
	public class BuildProvider : IGXCliVerbProvider
	{
		public string Name => "build";

		public string Description => "Build All for the working environment";

		public string Target => "Build";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ForceRebuild", Description = "Force rebuild the objects." },
			new VerbParameter { Name = "DoNotExecuteReorg", Description = "Do not execute reorg, just create it." },
			new VerbParameter { Name = "FailIfReorg", Description = "Generate, but do not make a build if a reorg is needed." },
			new VerbParameter { Name = "CompileMains", Description = "Compile all main object, if false only compile the Developer Menu." },
			new VerbParameter { Name = "DetailedNavigation", Description = "Show detailed navigation." },
		};

		public List<Example> Examples => new List<Example>
		{
			new Example{ Command = "gx build kbpath=C:\\Models\\MyKB forceRebuild", Description = "Rebulid All on your Knowledge Base" },
			new Example{ Command = "gx build kbpath=C:\\Models\\MyKB failIfReorg", Description = "Bulid All on your Knowledge Base, but fail if a database reorganization is found" }
		};
	}
}
