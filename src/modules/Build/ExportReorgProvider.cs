using System.Collections.Generic;
using common;
using gxcli.common;

namespace Build
{
	public class ExportReorgProvider : IGXCliVerbProvider
	{
		public string Name => "export-reorg";

		public string Description => "Export the most recent reorganization.";

		public string Target => "GetReorgData,ExportReorg";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "Destination", Description = "Path where the reorganizationa will be exported to."},
			new VerbParameter { Name = "FileName", Description = "Name of the generated jar." },
			new VerbParameter { Name = "TargetJRE", Description = "The JRE version of the target environment (7,8,9)."},
		};

		public List<Example> Examples => new List<Example>()
		{
			new Example { Command = "gx export-reorg kbpath=C:\\Models\\MyKB targetjre=9", Description = "Exports a reorganization that will be executed on a target with JRE 9." }
		};
	}
}
