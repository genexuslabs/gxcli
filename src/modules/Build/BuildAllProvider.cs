using System.Collections.Generic;
using common;

namespace Build
{
	public class BuildAllProvider : IGXCliVerbProvider
	{
		public string Name => "build-all";

		public string Description => "Builds a Knowledge Base environment (Build All)";

		public string Target => "Build";

		public List<VerbParameter> Parameters => new List<VerbParameter> 
		{
			new VerbParameter { Name = "WorkingDirectory", Description = "Path to the Knowledge Base directory"},
			new VerbParameter { Name = "WorkingVersion", Description = "Name of the Knowledge Base version to build", Optional = true},
			new VerbParameter { Name = "WorkingEnvironment", Description = "Name of the Environment to build", Optional = true},
			new VerbParameter { Name = "ForceRebuild", Description = "Forces a ReBuild", Optional = true},
		};
	}
}
