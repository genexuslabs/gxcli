using System.Collections.Generic;
using common;
using gxcli.common;

namespace XPZ
{
	public class ImportProvider : IGXCliVerbProvider
	{
		public string Name => "Import";

		public string Description => "Imports an export file into the Knowledge Base.";

		public string Target => "Import";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "FilePath", Description = "Full path to the file to import.", Required = true }
		};

		public List<Example> Examples => new List<Example>
		{
		};
	}
}
