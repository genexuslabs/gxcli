using System.Collections.Generic;
using common;
using gxcli.common;

namespace XPZ
{
	public class ExportProvider : IGXCliVerbProvider
	{
		public string Name => "Export";

		public string Description => "Exports selected objects from the Knowledge Base to a file.";

		public string Target => "Export";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "FilePath", Description = "Full path to the file to import.", Required = true },
			new VerbParameter { Name = "ObjectNames", Description = "Objects to be exported.", Required = true},
			new VerbParameter { Name = "ReferenceType", Description = "Specifies the type of reference between an object and the referenced by this one.(None|Minimal|Hard|All)" }
		};

		public List<Example> Examples => new List<Example>
		{
		};
	}
}
