using System.Collections.Generic;
using common;
using gxcli.common;

namespace Build
{
	public class BuildOneProvider : IGXCliVerbProvider
	{
		public string Name => "build-one";

		public string Description => "Builds one object and the ones it calls for the working environment";

		public string Target => "BuildOne";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ObjectName", Description = "Object to compile.", Required = true },
			new VerbParameter { Name = "ForceRebuild", Description = "Force rebuild the object"},
			new VerbParameter { Name = "BuildCalled", Description = "Build only the Object if false, or the Object and all the called if true (Default)"},
			new VerbParameter { Name = "DetailedNavigation", Description = "Show detailed navigation"},
		};

		public List<Example> Examples => new List<Example>
		{
			new Example { Command = "gx build-one kbpath=C:\\Models\\MyKB ObjectName=MyMainWebPanel", Description = "Builds an object called MyMainWebPanel from MyKB Knowledge Base " }
		};
	}
}
