using System.Collections.Generic;
using common;

namespace Build
{
	public class BuildProvider : IGXCliVerbProvider
	{
		public string Name => "build";

		public string Description => "Builds a Knowledge Base environment (Build All)";

		public string Target => "Build";

		public List<VerbParameter> Parameters => new List<VerbParameter> 
		{
			new VerbParameter { Name = "WorkingDirectory", Description = "Path to the Knowledge Base directory", Required = true},
			new VerbParameter { Name = "WorkingVersion", Description = "Name of the Knowledge Base version to build" },
			new VerbParameter { Name = "WorkingEnvironment", Description = "Name of the Environment to build"},
			new VerbParameter { Name = "ForceRebuild", Description = "Forces a ReBuild"}
		};

		public List<Example> Examples => new List<Example>();
	}
}
