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
			new VerbParameter { Name = "FailIfReorg", Description = "If a reorg is needed just generate it and fail the task. Does not make a Build All." },
			new VerbParameter { Name = "CompileMains", Description = "Compile all main objects, if false only compile the Developer Menu." },
			new VerbParameter { Name = "DetailedNavigation", Description = "Show detailed navigation." },
		};

		public List<Example> Examples => new List<Example>
		{
			new Example{ Command = "gx build kbpath=C:\\Models\\MyKB forceRebuild", Description = "Rebuild All on your Knowledge Base" },
			new Example{ Command = "gx build kbpath=C:\\Models\\MyKB failIfReorg", Description = "Build All on your Knowledge Base, but fail if a database reorganization is found" }
		};
	}
}
