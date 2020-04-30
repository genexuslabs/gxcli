using System.Collections.Generic;
using common;

namespace gxcli.common
{
	public abstract class KBBasedVerbProvider
	{
		public static List<VerbParameter> KBParameters => new List<VerbParameter>
		{
			new VerbParameter { Name = "KBPath", Description = "Path to the Knowledge Base directory.", Required = true},
			new VerbParameter { Name = "Version", Description = "Name of the Knowledge Base version to use." },
			new VerbParameter { Name = "Environment", Description = "Name of the Environment to use."},
		};
	}
}
