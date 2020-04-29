using System.Collections.Generic;
using common;

namespace Build
{
	public class BuildOneProvider : IGXCliVerbProvider
	{
		public string Name => "build-one";

		public string Description => "Builds one object";

		public string Target => "BuildOne";

		public List<VerbParameter> Parameters => new List<VerbParameter>
		{
			new VerbParameter { Name = "WorkingDirectory", Description = "Path to the Knowledge Base directory", Required = true},
			new VerbParameter { Name = "WorkingVersion", Description = "Name of the Knowledge Base version to build"},
			new VerbParameter { Name = "WorkingEnvironment", Description = "Name of the Environment to build"},
			new VerbParameter { Name = "ObjectName", Description = "Name of the object to build", Required = true },
			new VerbParameter { Name = "ForceRebuild", Description = "Forces a ReBuild"}
		};

		public List<Example> Examples => new List<Example>
		{
			new Example { Command = "gx build-one workingDirectory=C:\\Models\\MyKB ObjectName=MyMainWebPanel", Description = "Builds an object called MyMainWebPanel from MyKB Knowledge Base " }
		};
	}
}
