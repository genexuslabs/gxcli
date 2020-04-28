using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			new VerbParameter { Name = "WorkingDirectory", Description = "Path to the Knowledge Base directory"},
			new VerbParameter { Name = "WorkingVersion", Description = "Name of the Knowledge Base version to build", Optional = true},
			new VerbParameter { Name = "WorkingEnvironment", Description = "Name of the Environment to build", Optional = true},
			new VerbParameter { Name = "ObjectName", Description = "Name of the object to build" },
			new VerbParameter { Name = "ForceRebuild", Description = "Forces a ReBuild", Optional = true},
		};
	}
}
